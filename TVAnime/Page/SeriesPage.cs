using System.Collections.Generic;
using System.Linq;
using TVAnime.Component;
using TVAnime.Helper;
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
                if (param.TryGetValue("SeriesId", out pId))
                {
                    var url = "https://anime1.me?cat=" + pId.ToString();
                    series = await Api.GetSeries(this, url);
                }
                else if (param.TryGetValue("EpisodeId", out pId))
                {
                    series = await Api.GetSeriesByEpisode(this, pId.ToString());
                }
                else if (param.TryGetValue("CategoryId", out pId))
                {
                    var url = "https://anime1.me/category/" + pId.ToString();
                    series = await Api.GetSeries(this, url);
                }
                HideLoading();

                var records = RecordHelper.GetAllRecords();
                episodes = series.Select(a =>
                {
                    var percentage = ((double?)records.FirstOrDefault(r => r.Id == a.Id.ToString())?.PlayTime ?? 0) / ((double?)records.FirstOrDefault(r => r.Id == a.Id.ToString())?.Duration ?? 0) * 100;
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
                    return new SelectionItem(title, param, (int)percentage);
                }).ToList();

                itemSelectionView.SetItemsSource(episodes);
            }
        }
    }
}
