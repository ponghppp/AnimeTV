using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;
using TVAnime.Models;
using static System.Net.WebRequestMethods;

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
                    return new SelectionItem(title, id, param);
                }).ToList();

                itemSelectionView.SetItemsSource(episodes);
            }
        }
    }
}
