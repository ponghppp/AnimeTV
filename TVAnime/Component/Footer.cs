using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using TVAnime.Models;
using System.Linq;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class Footer
    {
        public View view { get; set; }
        public Footer(int activeIndex)
        {
            var items = new FooterItem[] {
                new FooterItem("List"),
                new FooterItem("Record"),
            };
            View v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 80,
                BackgroundColor = Color.Black
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
                    BackgroundColor = idx == activeIndex ? Color.Blue : Color.White
                };
                button.Clicked += Clicked;
                v.Add(button);
            }
            view = v;
        }

        private void Clicked(object sender, ClickedEventArgs e)
        {
            var title = ((Button)sender).Text;
            BasePage page = new HomePage();
            switch (title)
            {
                case "List":
                    page.Init();
                    break;
                case "Record":
                    page = new RecordPage();
                    page.Init();
                    break;
            }
            Globals.currentPage.TransferToView(page);
        }

    }
}
