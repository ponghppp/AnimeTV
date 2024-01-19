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
            content.view.Add(itemSelectionView.view);
            view.Add(content.view);
            GetList();
        }

        private async void GetList()
        {
            if (param != null)
            {
                ShowLoading();
                var series = await Api.GetSeries(param["Id"].ToString());
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
                        ["Page"] = typeof(PlayerPage)
                    };
                    return new SelectionItem(title, id, param);
                }).ToList();

                itemSelectionView.collectionView.ItemsSource = episodes;
                if (episodes.Count > 0)
                {
                    itemSelectionView.collectionView.SelectedItem = episodes[0];
                }
            }
        }
    }
}
