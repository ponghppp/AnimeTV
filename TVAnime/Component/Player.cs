using System;
using System.Linq;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TVAnime.Helper;
using TVAnime.Page;

namespace TVAnime.Component
{
    internal class Player
    {
        public int currentTime { get; set; }
        public int duration { get; set; }
        private Timer timer { get; set; }
        public View view { get; set; }
        public VideoView videoView { get; set; }
        public bool isPlaying { get; set; }
        public View controlView { get; set; }
        public Progress progress { get; set; }
        public TextLabel progressLabel { get; set; }
        public string title { get; set; }
        public string id { get; set; }

        public Player(BasePage page, string title, string id)
        {
            this.id = id;
            this.title = title;
            page.OnKeyEvents += OnKeyEvent;

            currentTime = 0;
            timer = new Timer(1000);
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
            if (duration - currentTime <= 100)
            {
                timer.Stop();
            }
            currentTime += 1000;
            var percentage = ((float)currentTime / (float)duration) * 100;
            progress.CurrentValue = percentage;
            progressLabel.Text = TimeHelper.MillisecondsToMinute(currentTime) + "/" + TimeHelper.MillisecondsToMinute(duration);
            return true;
        }

        public void Play()
        {
            timer.Start();
            isPlaying = true;
            videoView.Play();
            controlView.Hide();
        }

        public void Pause()
        {
            timer.Stop();
            isPlaying = false;
            videoView.Pause();
            controlView.Show();
            RecordHelper.RecordVideoPlayTime(id, title, currentTime, duration);
        }

        public async void SetVideoSource(string source, int lastPlayTime = 0)
        {
            currentTime = lastPlayTime;
            videoView.ResourceUrl = source;
            var videoName = source.Split('/').LastOrDefault();
            duration = await VideoHelper.GetVideoDuration(videoName);
            progressLabel.Text = TimeHelper.MillisecondsToMinute(currentTime) + "/" + TimeHelper.MillisecondsToMinute(duration);
            Play();
        }

        private void SetupControlView()
        {
            controlView = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                BackgroundColor = Color.Black,
                Opacity = 0.8f,
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
                HeightSpecification = LayoutParamPolicies.WrapContent,
                BackgroundColor = Color.Black
            };
            var hLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                Padding = new Extents(20, 20, 0, 0)
            };
            headerView.Layout = hLayout;

            TextLabel headerLabel = new TextLabel()
            {
                Text = title,
                TextColor = Color.White,
                PointSize = 40,
                WidthResizePolicy = ResizePolicyType.SizeRelativeToParent,
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
            controlView.Add(contentView);

            //Bot
            View bottomView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.WrapContent,
                BackgroundColor = Color.Black
            };
            var bLayout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Horizontal,
                Padding = new Extents(20, 20, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                CellPadding = new Size2D(20, 0),
            };
            bottomView.Layout = bLayout;

            progress = new Progress()
            {
                MinValue = 0,
                MaxValue = 100,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 10,
                ProgressColor = Color.Magenta,
                TrackColor = Color.White,
            };
            bottomView.Add(progress);

            progressLabel = new TextLabel()
            {
                TextColor = Color.White,
                PointSize = 40
            };
            bottomView.Add(progressLabel);
            controlView.Add(bottomView);

            Tizen.NUI.Window.Instance.Add(controlView);
        }

        private void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Return")
                {
                    if (isPlaying) Pause();
                    else Play();
                }
                if (e.Key.KeyPressedName == "Right")
                {
                    videoView.Forward(10000);
                    currentTime = Math.Min(duration, currentTime + 10000);
                }
                if (e.Key.KeyPressedName == "Left")
                {
                    videoView.Backward(10000);
                    currentTime = Math.Max(0, currentTime - 10000);
                }
                if (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape")
                {
                    RecordHelper.RecordVideoPlayTime(id, title, currentTime, duration);
                    timer.Stop();
                    videoView.Stop();
                }
            }
        }
    }
}
