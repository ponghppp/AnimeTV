using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class Player
    {
        public View view { get; set; }
        public VideoView videoView { get; set; }
        public bool isPlaying { get; set; }

        public Player(BasePage page)
        {
            page.OnKeyEvents += OnKeyEvent;

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

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Return")
                {
                    if (isPlaying)
                    {
                        isPlaying = false;
                        videoView.Pause();
                    }
                    else
                    {
                        isPlaying = true;
                        videoView.Play();
                    }
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    videoView.Forward(10000);
                }
                if (e.Key.KeyPressedName == "Left")
                {
                    videoView.Backward(10000);
                }
                if (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape")
                {
                    videoView.Stop();
                }
            }
        }
    }
}
