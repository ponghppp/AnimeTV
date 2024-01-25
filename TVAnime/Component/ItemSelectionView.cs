using System;
using System.Linq;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;
using TVAnime.Models;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class ItemSelectionView
    {
        public int selectedIndex = 0;
        public View view { get; set; }
        public BasePage page { get; set; }
        public CollectionView collectionView { get; set; }
        
        public ItemSelectionView(BasePage page)
        {
            this.page = page;
            page.OnKeyEvents += OnKeyEvent;

            View v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };

            collectionView = new CollectionView()
            {
                ItemsLayouter = new LinearLayouter(),
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                SelectionMode = ItemSelectionMode.SingleAlways,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                
                ItemTemplate = new DataTemplate(() =>
                {
                    var item = new DefaultLinearItem()
                    {
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        HeightSpecification = LayoutParamPolicies.WrapContent
                    };
                    item.Label.TextColor = Color.Black;
                    item.Label.SetBinding(TextLabel.BackgroundColorProperty, "BackgroundColor");
                    item.Label.SetBinding(TextLabel.TextProperty, "Name");
                    return item;
                })
            };
            collectionView.SelectionChanged += SelectionChanged;
            v.Add(collectionView);
            view = v;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object item in e.PreviousSelection)
            {
                SelectionItem episode = (SelectionItem)item;
                episode.BackgroundColor = Color.White;
            }
            foreach (object item in e.CurrentSelection)
            {
                SelectionItem episode = (SelectionItem)item;
                episode.BackgroundColor = Color.Cyan;
            }
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && collectionView.ItemsSource != null)
            {
                var source = collectionView.ItemsSource.Cast<SelectionItem>().ToList();
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
                        selectedIndex = nextSelectedIndex;
                        collectionView.SelectedItem = source[selectedIndex];
                    }
                    if (selectedIndex == 0)
                    {
                        collectionView.ScrollToIndex(0);
                    }
                    else if (selectedIndex == source.Count - 1)
                    {
                        collectionView.ScrollToIndex(source.Count - 1);
                    }
                    else
                    {
                        var increment = e.Key.KeyPressedName == "Down" ? 1 : -1;
                        collectionView.ScrollTo(Math.Max(0, selectedIndex + increment), true, CollectionView.ItemScrollTo.Nearest);
                    }
                }
            }
        }
    }
}
