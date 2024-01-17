using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using Tizen.NUI;

namespace TVAnime.Component
{
    internal class Content
    {
        public View view { get; set; }

        public Content()
        {
            View v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.White
            };
            view = v;
        }
    }
}
