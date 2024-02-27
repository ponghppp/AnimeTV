using System.Collections.Generic;
using System.Linq;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class SearchResultPage : BasePage
    {
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header(param["Title"].ToString());
            var content = new Content();

            view.Add(header.view);
            itemSelectionView = new ItemSelectionView(this);
            content.view.Add(itemSelectionView.scrollView);
            view.Add(content.view);

            GetList();
        }

        private async void GetList()
        {
            ShowLoading();
            var categories = await Api.GetSearchResult(this, param["Title"].ToString());
            HideLoading();

            var episodes = categories.Select(a =>
            {
                var title = a.Title;
                var param = new Dictionary<string, object>()
                {
                    ["CategoryId"] = a.Id.ToString(),
                    ["Page"] = typeof(SeriesPage)
                };
                return new SelectionItem(title, param);
            }).ToList();

            itemSelectionView.SetItemsSource(episodes);
            if (episodes.Count == 0)
            {
                ShowRetry(GetList);
                return;
            }
            if (param["SelectedItemTitle"] != null && param["SelectedItemTitle"].ToString() != "")
            {
                var selectedItemTitle = param["SelectedItemTitle"].ToString();
                itemSelectionView.SetSelectedItem(selectedItemTitle);
            }
        }
    }
}
