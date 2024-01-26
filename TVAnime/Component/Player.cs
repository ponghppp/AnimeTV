using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TVAnime.Page;
using static Tizen.NUI.Timer;

namespace TVAnime.Component
{
    internal class Player
    {
        List<EventHandlerWithReturnType<object, TickEventArgs, bool>> delegates = new List<EventHandlerWithReturnType<object, TickEventArgs, bool>>();
        public int currentTime { get; set; }
        private Timer timer { get; set; }
        public View view { get; set; }
        public VideoView videoView { get; set; }
        public bool isPlaying { get; set; }
        public View controlView { get; set; }
        public ImageView statusImageView { get; set; }
        public Progress progress { get; set; }
        public TextLabel progressLabel { get; set; }
        public string title { get; set; }

        public Player(BasePage page, string title)
        {
            this.title = title;
            page.OnKeyEvents += OnKeyEvent;

            currentTime = 0;
            timer = new Timer(100);
            delegates.Add(Tick);
            timer.Tick += Tick;

            var v = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent
            };
            videoView = new VideoView()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = Color.Cyan,
            };
            v.Add(videoView);
            var pm = new Tizen.NUI.PropertyMap();
            pm.Add("left", new Tizen.NUI.PropertyValue(1.0f));
            pm.Add("right", new Tizen.NUI.PropertyValue(1.0f));
            videoView.Volume = pm;

            SetupControlView();
            view = v;
        }

        private bool Tick(object source, Timer.TickEventArgs e)
        {
            currentTime += 100;
            return true;
        }

        private void Play()
        {

        }

        private void SetupControlView()
        {
            controlView = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                BackgroundColor = Color.Black,
                Opacity = 0.2f,
            };
            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            controlView.Layout = layout;

            //Top
            View headerView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                BackgroundColor = Color.Black
            };

            TextLabel headerLabel = new TextLabel()
            {
                Text = title,
                TextColor = Color.White,
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                HorizontalAlignment = HorizontalAlignment.Begin,
                VerticalAlignment = VerticalAlignment.Center
            };
            headerView.Add(headerLabel);
            controlView.Add(headerView);

            //Mid
            View contentView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            var cLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            contentView.Layout = cLayout;
            statusImageView = new ImageView()
            {
                ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "pause.png",
                BackgroundColor = Color.White,
            };
            contentView.Add(statusImageView);

            //Bot
            View bottomBar = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 80,
                BackgroundColor = Color.Black
            };
            var bLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
            };
            bottomBar.Layout = bLayout;

            Progress progress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                ProgressColor = Color.Black,
                TrackColor = Color.White,
            };
            bottomBar.Add(progress);

            TextLabel progressLabel = new TextLabel()
            {
                //Text = 
            };
            bottomBar.Add(progressLabel);
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
