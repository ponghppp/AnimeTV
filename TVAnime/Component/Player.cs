using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;

namespace TVAnime.Component
{
    internal class Player
    {
        public View view { get; set; }
        public VideoView videoView { get; set; }

        public Player()
        {
            var v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };
            videoView = new VideoView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,

            };
            v.Add(videoView);
            var pm = new Tizen.NUI.PropertyMap();
            pm.Add("left", new Tizen.NUI.PropertyValue(1.0f));
            pm.Add("right", new Tizen.NUI.PropertyValue(1.0f));
            videoView.Volume = pm;
            view = v;
        }

    }
}
