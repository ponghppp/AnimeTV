using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public Action<string> searchAction { get; set; }
        public View view { get; set; }
        public Grid selectedGrid = new Grid(0, 0);
        public Grid previousSelectedGrid = new Grid(0, 0);
        public TextLabel inputLabel { get; set; }
        public View selectWordView { get; set; }
        public List<List<Button>> buttons = new List<List<Button>>() { };
        public List<Button> wordButtons { get; set; }
        public string searchText = "";
        public string input = "";
        public bool selectingWord = false;
        public int selectedIndex = 0;
        public int previousSelectedIndex = 0;
        public List<ButtonKey> quickWords = new List<ButtonKey>() { };

        public QuickKeyboard(BasePage page, Action<string> searchAction)
        {
            this.page = page;
            this.searchAction = searchAction;
            page.OnKeyEvents += OnKeyEvent;
            view = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent
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
                HeightSpecification = LayoutParamPolicies.MatchParent
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
                WidthSpecification = 100,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                PointSize = 50
            };
            selectWordContainer.Add(inputLabel);

            selectWordView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };
            var swLayout = new LinearLayout()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
            selectWordView.Layout = swLayout;
            selectWordContainer.Add(selectWordView);

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
                        Text = button.Word,
                        PointSize = button.Word.Length > 1 ? 30 : 40
                    };
                    btnRows.Add(btn);
                    vRows.Add(btn);
                }
                buttons.Add(btnRows);
                view.Add(vRows);
            }

            var lines = File.ReadAllLines(Constant.Resource + "ms_quick.dict.txt");
            quickWords = lines.Select(l => new ButtonKey(l.Split(' ').LastOrDefault(), l.Split(' ').FirstOrDefault())).ToList();
        }

        public void SelectItem()
        {
            if (selectedGrid != previousSelectedGrid) { 
                buttons[previousSelectedGrid.row][previousSelectedGrid.column].BackgroundColor = Color.White;
                previousSelectedGrid = selectedGrid;
                buttons[selectedGrid.row][selectedGrid.column].BackgroundColor = Color.Cyan;
            }
        }
        public void SelectWord(int selectedIndex, int previousSelectedIndex)
        {
            wordButtons[previousSelectedIndex].BackgroundColor = Color.White;
            wordButtons[selectedIndex].BackgroundColor = Color.Cyan;
        }
        public void MatchWords()
        {
            wordButtons.Clear();
            foreach (var v in selectWordView.Children)
            {
                selectWordView.Remove(v);
            }
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
                    PointSize = 30
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
                selectedIndex = Math.Max(0, selectedIndex - 1);
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
                SelectWord(selectedIndex, previousSelectedIndex);
                return;
            }
            selectedGrid.row = Math.Max(0, selectedGrid.row - 1);
        }

        private void DownAction()
        {
            selectingWord = false;
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
