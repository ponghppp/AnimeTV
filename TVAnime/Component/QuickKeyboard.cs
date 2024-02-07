using System;
using System.Collections.Generic;
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
        public Action searchAction { get; set; }
        public Action changeAction { get; set; }
        public TextLabel resultLabel { get; set; }
        public View view { get; set; }
        public Grid selectedGrid = new Grid(0, 0);
        public Grid previousSelectedGrid = new Grid(1, 1);
        public TextLabel inputLabel { get; set; }
        public View selectWordView { get; set; }
        public ScrollableBase selectWordScrollView { get; set; }
        public List<List<Button>> buttons = new List<List<Button>>() { };
        public List<Button> wordButtons = new List<Button>() { };
        public string input = "";
        public bool selectingWord = false;
        public int selectedIndex = 0;
        public int previousSelectedIndex = 0;
        public List<ButtonKey> quickWords = new List<ButtonKey>() { };
        public List<List<ButtonKey>> buttonKeys;

        public QuickKeyboard(BasePage page, TextLabel resultLabel, Action searchAction, Action changeAction)
        {
            this.page = page;
            this.resultLabel = resultLabel;
            this.searchAction = searchAction;
            this.changeAction = changeAction;
            buttonKeys = Constant.buttonKeys;
            buttonKeys.RemoveAt(0);

            ((SearchPage)page).keyEvent = OnKeyEvent;
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

            for (int i = 0; i < buttonKeys.Count; i++)
            {
                var btnRows = new List<Button>() { };
                var buttonRows = buttonKeys[i];
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
                selectedGrid.column = Math.Min(selectedGrid.column, buttons[selectedGrid.row].Count - 1);
                if (selectedGrid != previousSelectedGrid || force)
                {
                    buttons[previousSelectedGrid.row][previousSelectedGrid.column].BackgroundColor = Color.White;
                    buttons[selectedGrid.row][selectedGrid.column].BackgroundColor = Color.Cyan;
                    previousSelectedGrid = selectedGrid;
                }
            }
        }
        public void SelectWord()
        {
            if (selectingWord)
            {
                if (wordButtons.Count > 0)
                {
                    if (previousSelectedIndex <= wordButtons.Count - 1)
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
            var bks = buttonKeys.SelectMany(x => x).ToList();
            inputLabel.Text = string.Join("", input.Select(i => bks.FirstOrDefault(b => b.Key == i.ToString()).Word));
            if (input.Length == 0)
            {
                return;
            }
            selectedIndex = 0;
            previousSelectedIndex = 0;
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
            if (buttonKeys[selectedGrid.row][selectedGrid.column].Key == "delete")
            {
                if (input.Length != 0)
                {
                    input = input.Remove(Math.Max(0, input.Length - 1), input.Length == 0 ? 0 : 1);
                    MatchWords();
                }
                else
                {
                    resultLabel.Text = resultLabel.Text.Remove(Math.Max(0, resultLabel.Text.Length - 1), resultLabel.Text.Length == 0 ? 0 : 1);
                }
                return;
            }
            if (buttonKeys[selectedGrid.row][selectedGrid.column].Key == "change")
            {
                changeAction();
            }
            if (buttonKeys[selectedGrid.row][selectedGrid.column].Key == "search")
            {
                searchAction();
                return;
            }
            if (selectingWord)
            {
                input = "";
                resultLabel.Text += wordButtons[selectedIndex].Text;
                selectingWord = false;
                MatchWords();
            }
            else
            {
                if (input.Length >= 2) return;
                input += buttonKeys[selectedGrid.row][selectedGrid.column].Key;
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
                selectedGrid.column = Math.Min(buttonKeys[selectedGrid.row].Count - 1, selectedGrid.column + 1);
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
            selectedGrid.row = Math.Min(buttonKeys.Count - 1, selectedGrid.row + 1);
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
