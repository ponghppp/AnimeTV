using System;
using System.Collections.Generic;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;

namespace TVAnime.Page
{
    internal class SearchPage : BasePage
    {
        private TextLabel textLabel;
        private View keyboardView;
        private KeyboardType keyboardType;
        public EventHandler<Window.KeyEventArgs> keyEvent;
        private Content content;
        public override void Init()
        {
            var header = new Header();
            content = new Content();
            var cLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            content.view.Layout = cLayout;

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
            view.Add(content.view);

            AddKeyboard(KeyboardType.Quick);

            var footer = new Footer(3, this);
            footer.changePageAction = false;
            view.Add(footer.view);
        }

        private void AddKeyboard(KeyboardType type)
        {
            if (keyboardView != null)
            {
                content.view.Remove(keyboardView);
                OnKeyEvents -= keyEvent;
            }
            switch (type)
            {
                case KeyboardType.Quick:
                    keyboardType = KeyboardType.Quick;
                    var keyboard = new QuickKeyboard(this, textLabel, searchAction, changeAction);
                    keyboardView = keyboard.view;
                    content.view.Add(keyboard.view);
                    break;
                case KeyboardType.Alphanumeric:
                    keyboardType = KeyboardType.Alphanumeric;
                    var k = new AlphanumericKeyboard(this, textLabel, searchAction, changeAction);
                    keyboardView = k.view;
                    content.view.Add(k.view);
                    break;
            }
        }

        private void searchAction()
        {
            var pageType = typeof(SearchResultPage);
            var param = new Dictionary<string, object>()
            {
                ["Title"] = textLabel.Text,
                ["Page"] = pageType
            };
            TransferToView(pageType, param);
        }

        private void changeAction()
        {
            var allTypes = Enum.GetValues(typeof(KeyboardType)).Cast<int>().ToList();
            var idx = allTypes.IndexOf((int)keyboardType);
            var nextValue = idx + 1 > (allTypes.Count - 1) ? allTypes[0] : allTypes[idx + 1];
            AddKeyboard(Enum.Parse<KeyboardType>(nextValue.ToString()));
        }

    }
}
