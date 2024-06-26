﻿using System.Collections.Generic;
using System.Linq;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class SeasonPage : BasePage
    {
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var footer = new Footer(2, this);

            view.Add(header.view);
            itemSelectionView = new ItemSelectionView(this);
            content.view.Add(itemSelectionView.scrollView);
            view.Add(content.view);
            view.Add(footer.view);
            GetList();
        }

        public override async void GetList()
        {
            var selectedSeason = param["Title"].ToString();
            ShowLoading();
            var seasonsAnime = await Api.GetSeason(this, selectedSeason);
            HideLoading();

            var episodes = seasonsAnime.Select(a =>
            {
                var param = new Dictionary<string, object>()
                {
                    ["SeriesId"] = a.Id.ToString(),
                    ["Page"] = typeof(SeriesPage)
                };
                return new SelectionItem(a.Title, param);
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
