using System;
using System.Collections.Generic;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TVAnime.Models;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class ItemSelectionView
    {
        public int selectedIndex = 0;
        public int previousSelectedIndex = 0;
        public BasePage page { get; set; }
        public ScrollableBase scrollView { get; set; }
        public List<SelectionItem> ItemsSource { get; set; }
        public List<View> ItemsContainer { get; set; }
        public List<View> ItemsBg { get; set; }
        public List<TextLabel> Items { get; set; }

        public ItemSelectionView(BasePage page)
        {
            this.page = page;
            page.OnKeyEvents += OnKeyEvent;

            scrollView = new ScrollableBase()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };
            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            scrollView.Layout = layout;
        }
        public void SetItemsSource(List<SelectionItem> source)
        {
            this.ItemsSource = source;
            ItemsContainer = new List<View>() { };
            ItemsBg = new List<View>() { };
            Items = new List<TextLabel>() { };
            for (var i = 0; i < source.Count; i++)
            {
                var item = source[i];
                item.Percentage = 50;
                CreatePercentageView(item);
            }
            if (source.Count > 0)
            {
                SelectItem(0, 0);
            }
        }
        public void SetSelectedItem(string title)
        {
            var index = ItemsSource.FindIndex(i => i.Name == title);
            if (index > 0)
            {
                SelectItem(index, previousSelectedIndex);
            }
        }
        private void CreatePercentageView(SelectionItem item)
        {
            var vv = new View()
            {
                HeightResizePolicy = ResizePolicyType.FitToChildren,
                WidthResizePolicy = ResizePolicyType.FillToParent,
            };
            var v = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent,
            };
            v.Layout = new RelativeLayout();

            TextLabel textLabel = new TextLabel()
            {
                Text = item.Name,
                TextColor = Color.Black,
                PointSize = 50,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                Margin = new Extents(20, 20, 0, 0),
            };
            var bg = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthSpecification = (int)(1920 * ((double)item.Percentage / 100)),
                BackgroundColor = Color.Green
            };
            RelativeLayout.SetLeftTarget(bg, v);
            RelativeLayout.SetTopTarget(textLabel, bg);
            RelativeLayout.SetBottomTarget(textLabel, bg);

            RelativeLayout.SetLeftRelativeOffset(bg, 0.0f);
            RelativeLayout.SetTopRelativeOffset(textLabel, 0.0f);
            RelativeLayout.SetBottomRelativeOffset(textLabel, 0.0f);

            v.Add(bg);
            v.Add(textLabel);
            vv.Add(v);
            scrollView.Add(vv);

            ItemsContainer.Add(vv);
            ItemsBg.Add(bg);
            Items.Add(textLabel);
        }
        public void SelectItem(int selectedIndex, int previousSelectedIndex)
        {
            Items[previousSelectedIndex].TextColor = Color.Black;
            ItemsContainer[previousSelectedIndex].BackgroundColor = Color.Transparent;
            Items[selectedIndex].TextColor = Color.Red;
            ItemsContainer[selectedIndex].BackgroundColor = Color.Cyan;
            scrollView.ScrollTo(Items[0].SizeHeight * selectedIndex, true);
        }
        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && ItemsSource != null)
            {
                var source = ItemsSource.ToList();
                if (e.Key.KeyPressedName == "Return" && Items.Count > 0)
                {
                    var currentParam = Globals.GetCurrentPageParam();
                    if (currentParam != null)
                    {
                        currentParam["SelectedItemTitle"] = source[selectedIndex].Name;
                        Globals.UpdateCurrentPageParam(currentParam);
                    }
                    var param = source[selectedIndex].Param;
                    page.TransferToView((Type)param["Page"], param);
                    return;
                }
                var nextSelectedIndex = selectedIndex;
                if (e.Key.KeyPressedName == "Down")
                {
                    nextSelectedIndex = Math.Min(source.Count - 1, selectedIndex + 1);
                }
                if (e.Key.KeyPressedName == "Up")
                {
                    nextSelectedIndex = Math.Max(0, selectedIndex - 1);
                }
                if (e.Key.KeyPressedName == "Down" || e.Key.KeyPressedName == "Up")
                {
                    if (nextSelectedIndex != selectedIndex)
                    {
                        previousSelectedIndex = selectedIndex;
                        selectedIndex = nextSelectedIndex;
                        SelectItem(selectedIndex, previousSelectedIndex);
                    }
                }
            }
        }
    }
}
