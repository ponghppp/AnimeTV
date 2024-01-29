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
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var keyboard = new QuickKeyboard(this, searchAction);
            var footer = new Footer(3, this);

            view.Add(header.view);
            content.view.Add(keyboard.view);
            view.Add(content.view);
            view.Add(footer.view);
            GetList();
        }

        private void searchAction (string action)
        {

        }

        private async void GetList()
        {
            
        }
    }
}
