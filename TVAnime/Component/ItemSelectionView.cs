using System;
using System.Linq;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;
using TVAnime.Models;
using TVAnime.Page;
using System.Collections.Generic;

namespace TVAnime.Component
{
    internal class ItemSelectionView
    {
        public int selectedIndex = 0;
        public int previousSelectedIndex = 0;
        public BasePage page { get; set; }
        public ScrollableBase scrollView { get; set; }
        private View view { get; set; }
        public List<SelectionItem> ItemsSource { get; set; }
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

            view = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };

            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };

            view.Layout = layout;
            scrollView.Add(view);
        }

        public void SetItemsSource(List<SelectionItem> source)
        {
            this.ItemsSource = source;
            Items = new List<TextLabel>() { };

            for (var i = 0; i < source.Count; i++)
            {
                var item = source[i];

                if (item.Percentage > 0)
                {
                    CreatePercentageView(item);
                }
                else
                {
                    CreateView(item);
                }
            }

            if (source.Count > 0)
            {
                SelectItem(0, 0);
            }
        }

        private void CreateView(SelectionItem item)
        {
            TextLabel textLabel = new TextLabel()
            {
                Text = item.Name,
                TextColor = Color.Black,
                PointSize = 50,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.SizeRelativeToParent,
            };
            view.Add(textLabel);
            Items.Add(textLabel);
        }

        private void CreatePercentageView(SelectionItem item)
        {
            TextLabel textLabel = new TextLabel()
            {
                Text = item.Name,
                TextColor = Color.Black,
                PointSize = 50,
                Position = new Position(0, 0, 1),
                WidthResizePolicy = ResizePolicyType.FillToParent,
            };
            var v = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };
            var vLayout = new AbsoluteLayout();
            v.Layout = vLayout;

            var bg = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthSpecification = (int)(1920 * ((double)item.Percentage / 100)),
                Position = new Position(0, 0, 2),
                BackgroundColor = Color.Green
            };
            v.Add(bg);
            v.Add(textLabel);
            view.Add(v);

            Items.Add(textLabel);
        }

        public void SelectItem(int selectedIndex, int previousSelectedIndex)
        {
            Items[previousSelectedIndex].BackgroundColor = Color.Transparent; //Color.White;
            Items[selectedIndex].BackgroundColor = Color.Cyan;
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && ItemsSource != null)
            {
                var source = ItemsSource.ToList();
                if (e.Key.KeyPressedName == "Return")
                {
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
                        scrollView.ScrollTo(Items[0].SizeHeight * selectedIndex, true);
                    }
                }
            }
        }
    }
}
