using System.Collections.Generic;
using System.Linq;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class SeriesPage : BasePage
    {
        private List<SelectionItem> episodes = new List<SelectionItem>();
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            view.Add(header.view);

            itemSelectionView = new ItemSelectionView(this);
            content.view.Add(itemSelectionView.scrollView);
            view.Add(content.view);
            GetList();
        }

        private async void GetList()
        {
            if (param != null)
            {
                ShowLoading();
                List<Episode> series = new List<Episode>() { };
                object pId = "";
                var key = "";
                if (param.TryGetValue("SeriesId", out pId))
                {
                    key = "SeriesId";
                    var url = "https://anime1.me?cat=" + pId.ToString();
                    series = await Api.GetSeries(this, url);
                }
                else if (param.TryGetValue("EpisodeId", out pId))
                {
                    key = "EpisodeId";
                    series = await Api.GetSeriesByEpisode(this, pId.ToString());
                }
                else if (param.TryGetValue("CategoryId", out pId))
                {
                    key = "CategoryId";
                    var url = "https://anime1.me/category/" + pId.ToString();
                    series = await Api.GetSeries(this, url);
                }
                HideLoading();
                episodes = series.Select(a =>
                {
                    var title = a.Title;
                    var id = a.Id.ToString();
                    var param = new Dictionary<string, object>()
                    {
                        ["Id"] = id,
                        ["Title"] = title,
                        ["ApiReq"] = a.ApiReq,
                        ["Series"] = series,
                        ["Page"] = typeof(PlayerPage)
                    };
                    return new SelectionItem(title, param);
                }).ToList();

                itemSelectionView.SetItemsSource(episodes);
                if (episodes.Count == 0)
                {
                    ShowRetry(GetList);
                    return;
                }
                itemSelectionView.SetSelectedItem();
            }
        }
    }
}
