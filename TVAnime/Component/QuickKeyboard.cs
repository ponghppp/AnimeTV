using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TVAnime.Models;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class QuickKeyboard
    {
        public BasePage page { get; set; }
        public Action<string> selectWordAction { get; set; }
        public Action<string> searchAction { get; set; }
        public View view { get; set; }
        public Grid selectedGrid = new Grid(0, 0);
        public Grid previousSelectedGrid = new Grid(1, 1);
        public TextLabel inputLabel { get; set; }
        public View selectWordView { get; set; }
        public ScrollableBase selectWordScrollView { get; set; }
        public List<List<Button>> buttons = new List<List<Button>>() { };
        public List<Button> wordButtons = new List<Button>() { };
        public string searchText = "";
        public string input = "";
        public bool selectingWord = false;
        public int selectedIndex = 0;
        public int previousSelectedIndex = 0;
        public List<ButtonKey> quickWords = new List<ButtonKey>() { };

        public QuickKeyboard(BasePage page, Action<string> searchAction, Action<string> selectWordAction)
        {
            this.page = page;
            this.searchAction = searchAction;
            this.selectWordAction = selectWordAction;
            page.OnKeyEvents += OnKeyEvent;
            view = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                BackgroundColor = Color.White
            };
            var layout = new LinearLayout()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            view.Layout = layout;

            var selectWordContainer = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.White
            };
            var sLayout = new LinearLayout()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
            selectWordContainer.Layout = sLayout;

            inputLabel = new TextLabel()
            {
                WidthSpecification = 150,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                PointSize = 50
            };
            selectWordContainer.Add(inputLabel);

            selectWordScrollView = new ScrollableBase()
            {
                ScrollingDirection = ScrollableBase.Direction.Horizontal,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.White
            };
            CreateSelectWordView();
            selectWordContainer.Add(selectWordScrollView);
            view.Add(selectWordContainer);

            for (int i = 0; i < Constant.buttonKeys.Count; i++)
            {
                var btnRows = new List<Button>() { };
                var buttonRows = Constant.buttonKeys[i];
                var vRows = new View()
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent
                };
                var vlayout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                vRows.Layout = vlayout;
                for (int j = 0; j < buttonRows.Count; j++)
                {
                    var button = buttonRows[j];
                    var btn = new Button()
                    {
                        WidthSpecification = LayoutParamPolicies.WrapContent,
                        HeightSpecification = LayoutParamPolicies.WrapContent,
                        Margin = new Extents(10, 10, 10, 10),
                        Text = button.Word,
                        PointSize = button.Word.Length > 1 ? 30 : 40,
                        TextColor = Color.Black
                    };
                    btnRows.Add(btn);
                    vRows.Add(btn);
                }
                buttons.Add(btnRows);
                view.Add(vRows);
            }

            var lines = File.ReadAllLines(Constant.Resource + "ms_quick.dict.txt", new UTF8Encoding(true));
            quickWords = lines.Select(l => new ButtonKey(l.Split('\t').LastOrDefault(), l.Split('\t').FirstOrDefault())).ToList();
            SelectItem();

        }

        private void CreateSelectWordView()
        {
            if (selectWordView != null)
            {
                selectWordScrollView.Remove(selectWordView);
            }

            selectWordView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.White
            };
            var swLayout = new LinearLayout()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
            selectWordView.Layout = swLayout;
            selectWordScrollView.Add(selectWordView);
        }

        public void SelectItem(bool force = false)
        {
            if (!selectingWord)
            {
                if (buttons[selectedGrid.row].Count < selectedGrid.column)
                {
                    selectedGrid.column = buttons[selectedGrid.row].Count - 1;
                }
                if (selectedGrid != previousSelectedGrid || force)
                {
                    buttons[previousSelectedGrid.row][previousSelectedGrid.column].BackgroundColor = Color.White;
                    previousSelectedGrid = selectedGrid;
                    buttons[selectedGrid.row][selectedGrid.column].BackgroundColor = Color.Cyan;
                }
            }
        }
        public void SelectWord()
        {
            if (selectingWord)
            {
                if (wordButtons.Count > 0)
                {
                    if (previousSelectedIndex <= wordButtons.Count)
                    {
                        wordButtons[previousSelectedIndex].BackgroundColor = Color.White;
                    }
                    previousSelectedIndex = selectedIndex;
                    wordButtons[selectedIndex].BackgroundColor = Color.Cyan;
                    selectWordScrollView.ScrollTo(selectedIndex * wordButtons[0].SizeWidth, true);
                }
            }
        }
        public void MatchWords()
        {
            wordButtons.Clear();
            CreateSelectWordView();
            var buttonKeys = Constant.buttonKeys.SelectMany(x => x).ToList();
            inputLabel.Text = string.Join("", input.Select(i => buttonKeys.FirstOrDefault(b => b.Key == i.ToString()).Word));
            if (input.Length == 0)
            {
                return;
            }
            var matches = quickWords.Where(w => w.Key.Contains(input)).ToList();
            var words = matches.Select(m => m.Word).ToList();
            foreach (var v in words)
            {
                var btn = new Button()
                {
                    WidthSpecification = LayoutParamPolicies.WrapContent,
                    HeightSpecification = LayoutParamPolicies.WrapContent,
                    Text = v,
                    PointSize = 30,
                    TextColor = Color.Black,
                    Margin = new Extents(10, 10, 0, 0),
                };
                wordButtons.Add(btn);
                selectWordView.Add(btn);
            }
        }

        private void EnterAction()
        {
            if (Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key == "delete")
            {
                if (input.Length != 0)
                {
                    input = input.Remove(Math.Max(0, input.Length - 1), input.Length == 0 ? 0 : 1);
                    MatchWords();
                }
                else
                {
                    searchText = searchText.Remove(Math.Max(0, searchText.Length - 1), searchText.Length == 0 ? 0 : 1);
                }
                return;
            }
            if (Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key == "search")
            {
                searchAction(input);
                return;
            }
            if (selectingWord)
            {
                input = "";
                searchText += wordButtons[selectedIndex].Text;
                selectWordAction(searchText);
                selectingWord = false;
                MatchWords();
            }
            else
            {
                input += Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key;
                MatchWords();
            }
        }

        private void LeftAction()
        {
            if (selectingWord)
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
                SelectWord();
            }
            else
            {
                selectedGrid.column = Math.Max(0, selectedGrid.column - 1);
            }
        }

        private void RightAction()
        {
            if (selectingWord)
            {
                selectedIndex = Math.Min(wordButtons.Count - 1, selectedIndex + 1);
                SelectWord();
            }
            else
            {
                selectedGrid.column = Math.Min(Constant.buttonKeys[selectedGrid.row].Count - 1, selectedGrid.column + 1);
            }
        }

        private void UpAction()
        {
            if (selectedGrid.row == 0 || selectingWord)
            {
                selectingWord = true;
                buttons[selectedGrid.row][selectedGrid.column].BackgroundColor = Color.White;
                SelectWord();
                return;
            }
            selectedGrid.row = Math.Max(0, selectedGrid.row - 1);
        }

        private void DownAction()
        {
            if (selectingWord)
            {
                selectingWord = false;
                wordButtons[selectedIndex].BackgroundColor = Color.White;
                SelectItem(true);
                return;
            }
            selectedGrid.row = Math.Min(Constant.buttonKeys.Count - 1, selectedGrid.row + 1);
        }

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Return")
                {
                    EnterAction();
                    return;
                }
                if (e.Key.KeyPressedName == "Left")
                {
                    LeftAction();
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    RightAction();
                }
                if (e.Key.KeyPressedName == "Down")
                {
                    DownAction();
                }
                if (e.Key.KeyPressedName == "Up")
                {
                    UpAction();
                }
                SelectItem();
            }
        }
    }
}
