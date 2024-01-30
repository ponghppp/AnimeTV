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
            new FooterItem("列表", typeof(HomePage)),
            new FooterItem("記錄", typeof(RecordPage)),
            new FooterItem("新番", typeof(BangumiPage)),
            new FooterItem("搜尋", typeof(SearchPage)),
        };

        public BasePage currentPage { get; set; }
        public View view { get; set; }
        public int activeIndex { get; set; }
        public bool changePageAction = true;

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
                v.Add(button);
            }
            view = v;
            this.currentPage.OnKeyEvents += OnKeyEvent;
        }

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && changePageAction)
            {
                var nextIndex = activeIndex;
                if (e.Key.KeyPressedName == "Left")
                {
                    nextIndex = Math.Max(0, activeIndex - 1);
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    nextIndex = Math.Min(items.Count - 1, activeIndex + 1);
                }
                if (nextIndex != activeIndex)
                {
                    var pageType = items[nextIndex].PageType;
                    this.currentPage.TransferToView(pageType);
                }
            }
        }
    }
}
