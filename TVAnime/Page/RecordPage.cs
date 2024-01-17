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
    internal class RecordPage: BasePage
    {
        public override void Init()
        {
            base.Init();
            var header = new Header();
            var content = new Content();
            var footer = new Footer(1);

            view.Add(header.view);
            view.Add(content.view);
            view.Add(footer.view);
        }

    }
}
