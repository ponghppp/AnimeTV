using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;

namespace TVAnime.Models
{
    internal class Episode
    {
        public string Name { get; set; }
        public Color BackgroundColor { get; set; }

        public Episode(string name) {
            this.Name = name;
            this.BackgroundColor = Color.White;
        }
    }
}
