using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class BangumiPage : BasePage
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
        private void GetList()
        {
            var items = new List<SelectionItem> { };
            var currentYear = DateTime.Now.Year;
            for (int i = 0; i < 10; i++)
            {
                var year = (currentYear - i).ToString();
                var param = new Dictionary<string, object>()
                {
                    ["SelectedYear"] = year,
                    ["Page"] = typeof(BangumiYearPage)
                };
                var item = new SelectionItem(year, param);
                items.Add(item);
            }
            itemSelectionView.SetItemsSource(items);
            if (items.Count == 0)
            {
                ShowRetry(GetList);
                return;
            }
            itemSelectionView.SetSelectedItem();
        }
    }
}
