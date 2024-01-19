using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class SeriesPage: BasePage
    {
        public int selectedIndex = 0;
        private List<SelectionItem> episodes = new List<SelectionItem>();
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();

            view.Add(header.view);

            itemSelectionView = new ItemSelectionView(this);
            GetList();
            content.view.Add(itemSelectionView.view);
            view.Add(content.view);
        }

        private void GetList()
        {

        }
    }
}
