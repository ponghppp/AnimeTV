using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private List<SelectionItem> episodes = new List<SelectionItem>();
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var footer = new Footer(0, this);
            view.Add(header.view);

            itemSelectionView = new ItemSelectionView(this);
            content.view.Add(itemSelectionView.scrollView);
            view.Add(content.view);
            view.Add(footer.view);
            GetList();
        }

        private async void GetList()
        {
            ShowLoading();
            var latestAnimeList = await Api.GetLatestList(this);
            HideLoading();
           
            episodes = latestAnimeList.Select(a => 
            {
                var title = a.animeName + " " + a.episode;
                var id = a.categoryId.ToString();
                var param = new Dictionary<string, object>()
                {
                    ["Id"] = a.categoryId.ToString(),
                    ["Page"] = typeof(SeriesPage)
                };
                return new SelectionItem(title, id, param);
            }).ToList();

            itemSelectionView.SetItemsSource(episodes);
        }
    }
}
