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
        private List<SelectionItem> episodes = new List<SelectionItem>();
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var footer = new Footer(0, this);

            view.Add(header.view);

            itemSelectionView = new ItemSelectionView(this);
            GetList();
            content.view.Add(itemSelectionView.view);
            view.Add(content.view);

            view.Add(footer.view);
        }

        private async void GetList()
        {
            var latestAnimeList = await Api.GetLatestList();
            episodes = latestAnimeList.Select(a => new SelectionItem(a.animeName + " " + a.episode, a.categoryId.ToString())).ToList();

            itemSelectionView.collectionView.ItemsSource = episodes;
            if (episodes.Count > 0)
            {
                selectedIndex = 0;
                itemSelectionView.collectionView.SelectedItem = episodes[selectedIndex];
            }
        }
    }
}
