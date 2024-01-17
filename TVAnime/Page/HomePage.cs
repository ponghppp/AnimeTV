using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Binding;
using Tizen.NUI.Components;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class HomePage : BasePage
    {
        public int selectedIndex = 0;
        private List<Episode> episodes = new List<Episode>();
        private CollectionView collectionView = new CollectionView();
        public override void Init()
        {
            base.Init();
            var header = new Header();
            var content = new Content();
            var footer = new Footer(0);

            view.Add(header.view);

            collectionView = new CollectionView()
            {
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                SelectionMode = ItemSelectionMode.SingleAlways,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                ItemsLayouter = new LinearLayouter(),
                ItemsSource = episodes,
                ItemTemplate = new DataTemplate(() =>
                {
                    var item = new DefaultLinearItem()
                    {
                        WidthSpecification = LayoutParamPolicies.MatchParent,
                        HeightSpecification = LayoutParamPolicies.WrapContent,
                        Padding = new Extents(10, 10, 0, 0)
                    };

                    item.Label.TextColor = Color.Black;
                    item.Label.SetBinding(TextLabel.BackgroundColorProperty, "BackgroundColor");
                    item.Label.SetBinding(TextLabel.TextProperty, "Name");
                    return item;
                })
            };
            collectionView.SelectionChanged += SelectionChanged;
            GetList();
            content.view.Add(collectionView);
            view.Add(content.view);
            view.Add(footer.view);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                episodes.ForEach(ep => ep.BackgroundColor = Color.White);
                foreach (object item in e.CurrentSelection)
                {
                    Episode episode = (Episode)item;
                    episode.BackgroundColor = Color.Cyan;
                }
                collectionView.ItemsSource = episodes;
            }
        }

        private void GetList()
        {
            episodes = new List<Episode>() { new Episode("Hi"), new Episode("Bye") };
            collectionView.ItemsSource = episodes;
            if (episodes.Count > 0)
            {
                selectedIndex = 0;
                collectionView.SelectedItem = episodes[selectedIndex];
            }
        }

        public override void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            base.OnKeyEvent(sender, e);

            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "Down"))
            {
                selectedIndex = Math.Min(episodes.Count - 1, selectedIndex + 1);
                collectionView.SelectedItem = episodes[selectedIndex];
            }
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "Up"))
            {
                selectedIndex = Math.Max(0, selectedIndex - 1);
                collectionView.SelectedItem = episodes[selectedIndex];
            }
        }


    }
}
