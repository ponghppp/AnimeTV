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
    internal class BangumiYearPage : BasePage
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

        public override void GetList()
        {
            var selectedYear = int.Parse(param["SelectedYear"].ToString());
            var items = new List<SelectionItem> { };
            var seasons = new List<string>() { "冬", "春", "夏", "秋" };
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var currentSeasonIdx = Math.Ceiling((double)currentMonth / 3) - 1;
            if (selectedYear == currentYear)
            {
                seasons = seasons.Where(s => seasons.IndexOf(s) <= currentSeasonIdx).ToList();
            }
            items = seasons.Select((v, i) => new { value = v, index = i }).Select(s =>
            {
                var value = $"{selectedYear}年{s.value}季新番";
                var title = value + $" ({(s.index * 3) + 1} - {(s.index * 3) + 3}月)";
                var param = new Dictionary<string, object>()
                {
                    ["Title"] = value,
                    ["Page"] = typeof(SeasonPage)
                };
                return new SelectionItem(title, param);
            }).Reverse().ToList();

            itemSelectionView.SetItemsSource(items);
        }
    }
}