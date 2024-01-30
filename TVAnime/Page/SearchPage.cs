using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class SearchPage : BasePage
    {
        public TextLabel textLabel;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var cLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            content.view.Layout = cLayout;
            var keyboard = new QuickKeyboard(this, searchAction, selectWordAction);
            var footer = new Footer(3, this);
            footer.changePageAction = false;

            view.Add(header.view);
            textLabel = new TextLabel()
            {
                TextColor = Color.Black,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 300,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            content.view.Add(textLabel);
            content.view.Add(keyboard.view);
            view.Add(content.view);
            view.Add(footer.view);
        }

        private void searchAction(string action)
        {
            //search btn pressed
        }

        private void selectWordAction(string action)
        {
            textLabel.Text = action;
        }
    }
}
