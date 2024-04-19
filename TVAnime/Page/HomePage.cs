using System.Collections.Generic;
using System.Linq;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class HomePage : BasePage
    {
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

        public override async void GetList()
        {
            ShowLoading();
            var latestAnimeList = await Api.GetLatestList(this);
            HideLoading();

            var episodes = latestAnimeList.Take(200).Select(a =>
            {
                var title = a.animeName + " " + a.episode;
                var param = new Dictionary<string, object>()
                {
                    ["SeriesId"] = a.categoryId.ToString(),
                    ["Page"] = typeof(SeriesPage)
                };
                return new SelectionItem(title, param);
            }).ToList();

            itemSelectionView.SetItemsSource(episodes);
            if (episodes.Count == 0)
            {
                ShowRetry();
                return;
            }
            itemSelectionView.SetSelectedItem();
        }
    }
}
