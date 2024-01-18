using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;

namespace TVAnime.Page
{
    internal class BangumiPage : BasePage
    {
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var footer = new Footer(2, this);

            view.Add(header.view);
            view.Add(content.view);
            view.Add(footer.view);
        }

    }
}
