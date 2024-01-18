using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using TVAnime.Models;
using System.Linq;
using TVAnime.Page;
using System;
using System.Collections.Generic;

namespace TVAnime.Component
{


    internal class Footer
    {
        List<FooterItem> items = new List<FooterItem>
        {
            new FooterItem("List"),
            new FooterItem("Record"),
            new FooterItem("Bangumi"),
        };

        public BasePage currentPage { get; set; }
        public View view { get; set; }
        public int activeIndex { get; set; }

        public Footer(int activeIndex, BasePage currentPage)
        {
            this.activeIndex = activeIndex;
            this.currentPage = currentPage;

            View v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 80
            };

            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal
            };
            v.Layout = layout;

            foreach (var e in items.Select((val, idx) => new { item = val, index = idx }))
            {
                var item = e.item;
                var idx = e.index;
                var button = new Button()
                {
                    Text = item.Title,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    BackgroundColor = idx == activeIndex ? Color.Blue : Color.Black,
                    TextColor = Color.White
                };
                button.Clicked += Clicked;
                v.Add(button);
            }
            view = v;
            this.currentPage.OnKeyEvents += OnKeyEvent;
        }

        private Type GetPageTypeByTitle(string title)
        {
            Type pageType = typeof(HomePage);
            switch (title)
            {
                case "List":
                    pageType = typeof(HomePage);
                    break;
                case "Record":
                    pageType = typeof(RecordPage);
                    break;
                case "Bangumi":
                    pageType = typeof(BangumiPage);
                    break;
            }
            return pageType;
        }

        private void Clicked(object sender, ClickedEventArgs e)
        {
            var title = ((Button)sender).Text;
            var pageType = GetPageTypeByTitle(title);
            this.currentPage.TransferToView(pageType);
        }

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                var currentIndex = activeIndex;
                if (e.Key.KeyPressedName == "Left")
                {
                    currentIndex = Math.Max(0, activeIndex - 1);
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    currentIndex = Math.Min(items.Count - 1, activeIndex + 1);
                }
                if (currentIndex != activeIndex)
                {
                    var title = items[currentIndex].Title;
                    var pageType = GetPageTypeByTitle(title);
                    this.currentPage.TransferToView(pageType);
                }
            }
        }
    }
}
