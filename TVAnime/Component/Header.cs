using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace TVAnime.Component
{
    internal class Header
    {
        public View view { get; set; }

        public Header(string headerText = "AnimeTV")
        {
            View v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 80,
                BackgroundColor = Color.Black
            };

            TextLabel label = new TextLabel()
            {
                Text = headerText,
                TextColor = Color.White,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            v.Add(label);
            view = v;
        }

    }
}
