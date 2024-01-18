using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public BasePage page{ get; set; }
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
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                SelectionMode = ItemSelectionMode.SingleAlways,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemsLayouter = new LinearLayouter(),
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
            if (e.CurrentSelection.Count > 0)
            {
                var source = collectionView.ItemsSource.Cast<SelectionItem>().ToList();

                source.ForEach(s => s.BackgroundColor = Color.White);
                foreach (object item in e.CurrentSelection)
                {
                    SelectionItem episode = (SelectionItem)item;
                    episode.BackgroundColor = Color.Cyan;
                }
                collectionView.ItemsSource = source;
            }
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Down")
                {
                    var source = collectionView.ItemsSource.Cast<SelectionItem>().ToList();
                    selectedIndex = Math.Min(source.Count - 1, selectedIndex + 1);
                    collectionView.SelectedItem = source[selectedIndex];
                }
                if (e.Key.KeyPressedName == "Up")
                {
                    var source = collectionView.ItemsSource.Cast<SelectionItem>().ToList();
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    collectionView.SelectedItem = source[selectedIndex];
                }
            }
        }
    }
}
