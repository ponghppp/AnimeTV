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
    internal class AlphanumericKeyboard
    {
        public BasePage page { get; set; }
        public Action searchAction { get; set; }
        public Action changeAction { get; set; }
        public TextLabel resultLabel { get; set; }
        public View view { get; set; }
        public Grid selectedGrid = new Grid(0, 0);
        public Grid previousSelectedGrid = new Grid(1, 1);
        public List<List<Button>> buttons = new List<List<Button>>() { };

        public AlphanumericKeyboard(BasePage page, TextLabel resultLabel, Action searchAction, Action changeAction)
        {
            this.page = page;
            this.resultLabel = resultLabel;
            this.searchAction = searchAction;
            this.changeAction = changeAction;
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
                        Text = button.Key.Length > 1 ? button.Word : button.Key,
                        PointSize = button.Key.Length > 1 ? 30 : 40,
                        TextColor = Color.Black
                    };
                    btnRows.Add(btn);
                    vRows.Add(btn);
                }
                buttons.Add(btnRows);
                view.Add(vRows);
            }

            SelectItem();
        }

        public void SelectItem(bool force = false)
        {

            selectedGrid.column = Math.Min(selectedGrid.column, buttons[selectedGrid.row].Count - 1);
            if (selectedGrid != previousSelectedGrid || force)
            {
                buttons[previousSelectedGrid.row][previousSelectedGrid.column].BackgroundColor = Color.White;
                previousSelectedGrid = selectedGrid;
                buttons[selectedGrid.row][selectedGrid.column].BackgroundColor = Color.Cyan;
            }
        }
        private void EnterAction()
        {
            if (Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key == "delete")
            {
                resultLabel.Text = resultLabel.Text.Remove(Math.Max(0, resultLabel.Text.Length - 1), resultLabel.Text.Length == 0 ? 0 : 1);
                return;
            }
            if (Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key == "change")
            {
                changeAction();
                return;
            }
            if (Constant.buttonKeys[selectedGrid.row][selectedGrid.column].Key == "search")
            {
                searchAction();
                return;
            }
            resultLabel.Text += buttons[selectedGrid.row][selectedGrid.column].Text;
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
                    selectedGrid.column = Math.Max(0, selectedGrid.column - 1);
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    selectedGrid.column = Math.Min(Constant.buttonKeys[selectedGrid.row].Count - 1, selectedGrid.column + 1);
                }
                if (e.Key.KeyPressedName == "Down")
                {
                    selectedGrid.row = Math.Min(Constant.buttonKeys.Count - 1, selectedGrid.row + 1);
                }
                if (e.Key.KeyPressedName == "Up")
                {
                    selectedGrid.row = Math.Max(0, selectedGrid.row - 1);
                }
                SelectItem();
            }
        }
    }
}
