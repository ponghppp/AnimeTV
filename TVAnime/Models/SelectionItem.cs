using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;

namespace TVAnime.Models
{
    internal class SelectionItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Color BackgroundColor { get; set; }

        public SelectionItem(string name, string id)
        {
            this.Name = name;
            this.BackgroundColor = Color.White;
            this.Id = id;
        }
    }
}
