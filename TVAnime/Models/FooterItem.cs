﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;

namespace TVAnime.Models
{
    public class FooterItem
    {
        public string Title { get; set; }
        public Type PageType { get; set; }

        public FooterItem(string Title, Type pageType)
        {
            this.Title = Title;
            this.PageType = pageType;
        }
    }
}
