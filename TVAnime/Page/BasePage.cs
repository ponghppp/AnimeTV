using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using System.Collections.Generic;
using TVAnime.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Runtime.CompilerServices;
using System.Linq;
using Tizen.Applications;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;

namespace TVAnime.Page
{
    internal abstract class BasePage
    {
        List<EventHandler<Window.KeyEventArgs>> delegates = new List<EventHandler<Window.KeyEventArgs>>();
        public View view { get; set; }
        public View loadingView { get; set; }
        public TextLabel loadingViewLabel { get; set; }
        public Window window = Window.Instance;
        public Dictionary<string, object> param { get; set; }
        public HttpClient client { get; set; }

        public BasePage()
        {
            this.OnKeyEvents += OnBackPressed;

            View v = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };
            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };

            v.Layout = layout;
            view = v;
        }

        public virtual void Init() { }

        public async void TransferToView(Type pageType, Dictionary<string, object> param = null, bool addStack = true)
        {
            if (client != null)
            {
                client.CancelPendingRequests();
            }
            ShowLoading();
            await Task.Delay(500);
            if (Globals.pageStacks.Count >= 2)
            {
                var previousStack = Globals.pageStacks[Globals.pageStacks.Count - 2];
                var previousStackPageType = previousStack.Keys.FirstOrDefault();
                if (pageType == previousStackPageType)
                {
                    GoBack();
                    return;
                }
            }
            if (pageType != this.GetType())
            {
                Unload();
                BasePage page = (BasePage)Activator.CreateInstance(pageType);
                window.Add(page.view);
                page.param = param;
                if (addStack)
                {
                    Globals.AddPageStack(pageType, param);
                }
                page.Init();
            }
            HideLoading();
        }

        public void ShowLoading()
        {
            if (loadingView == null)
            {
                loadingView = new View()
                {
                    WidthResizePolicy = ResizePolicyType.FillToParent,
                    HeightResizePolicy = ResizePolicyType.FillToParent,
                    BackgroundColor = Color.Black,
                    Opacity = 0.8f
                };
                var loadingLayout = new LinearLayout()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                loadingView.Layout = loadingLayout;
                View container = new View()
                {
                    WidthResizePolicy = ResizePolicyType.FitToChildren,
                    HeightResizePolicy = ResizePolicyType.FitToChildren,
                };
                var containerLayout = new LinearLayout()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Vertical
                };
                container.Layout = containerLayout;
                loadingViewLabel = new TextLabel()
                {
                    Text = "載入中...",
                    TextColor = Color.White,
                };
                ImageView loadingImageView = new ImageView() 
                {
                    ResourceUrl = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "little_twin_stars.gif"
                };
                container.Add(loadingImageView);
                container.Add(loadingViewLabel);
                loadingView.Add(container);
            }

            window.Add(loadingView);
        }

        public void HideLoading()
        {
            window.Remove(loadingView);
        }

        private void Unload()
        {
            foreach (var eh in delegates)
            {
                window.KeyEvent -= eh;
            }
            delegates.Clear();
            window.Remove(this.view);
        }

        private void GoBack()
        {
            var previousPageType = Globals.pageStacks[Globals.pageStacks.Count - 2];
            Globals.pageStacks.RemoveAt(Globals.pageStacks.Count - 1);
            var key = previousPageType.Keys.FirstOrDefault();
            var value = previousPageType.Values.FirstOrDefault();
            TransferToView(key, value, false);
        }

        public event EventHandler<Window.KeyEventArgs> OnKeyEvents
        {
            add
            {
                delegates.Add(value);
                window.KeyEvent += value;
            }
            remove
            {
                delegates.Remove(value);
                window.KeyEvent -= value;
            }
        }

        public virtual void OnBackPressed(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                if (Globals.pageStacks.Count == 1)
                {
                    NUIApplication.Current.Exit();
                }
                else
                {
                    GoBack();
                }
            }
        }
    }
}
