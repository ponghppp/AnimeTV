using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;
using TVAnime.Helper;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class RecordPage : BasePage
    {
        private ItemSelectionView itemSelectionView;
        public override void Init()
        {
            var header = new Header();
            var content = new Content();
            var footer = new Footer(1, this);
            view.Add(header.view);

            itemSelectionView = new ItemSelectionView(this);
            content.view.Add(itemSelectionView.scrollView);
            view.Add(content.view);
            view.Add(footer.view);
            GetList();
        }
        private void GetList()
        {
            var records = RecordHelper.GetAllRecords();
            var rs = records.GroupBy(r => r.CategoryId).Select(g => g.OrderByDescending(r => r.Id).FirstOrDefault()).Select(a =>
            {
                var title = a.Title;
                var id = a.Id;
                var percentage = (double)a.PlayTime / (double)a.Duration * 100;
                var param = new Dictionary<string, object>()
                {
                    ["EpisodeId"] = id,
                    ["Title"] = title,
                    ["Page"] = typeof(SeriesPage)
                };
                return new SelectionItem(title, param, (int)percentage);
            }).Reverse().ToList();

            itemSelectionView.SetItemsSource(rs);
        }
    }
}
