using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal abstract class BasePage
    {
        List<EventHandler<Window.KeyEventArgs>> delegates = new List<EventHandler<Window.KeyEventArgs>>();
        public View view { get; set; }
        public View retryView { get; set; }
        public Action retryAction { get; set; }
        public View loadingView { get; set; }
        public TextLabel loadingViewLabel { get; set; }
        public Window window = Window.Instance;
        public Dictionary<string, object> param { get; set; }
        public HttpClient client { get; set; }
        public IList list { get; set; }

        public BasePage()
        {
            this.OnKeyEvents += OnBackPressed;

            view = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };
            var layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical
            };
            view.Layout = layout;
        }

        public virtual void Init() {}

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

        public void ShowRetry(Action retryAction)
        {
            if (retryAction != null) this.retryAction = retryAction;

            retryView = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                BackgroundColor = Color.Black,
                Opacity = 0.8f
            };
            var retryLayout = new LinearLayout()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            retryView.Layout = retryLayout;
            var button = new Button()
            {
                Text = "重試",
                TextColor = Color.White,
            };
            retryView.Add(button);
            window.Add(retryView);
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
                    TextColor = Color.White,
                };
                ImageView loadingImageView = new ImageView()
                {
                    ResourceUrl = Constant.Resource + "little_twin_stars.gif"
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

        public virtual async void OnBackPressed(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                if (retryView != null)
                {
                    window.Remove(retryView);
                }
                await Task.Delay(100);
                if (Globals.pageStacks.Count == 1)
                {
                    NUIApplication.Current.Exit();
                }
                else
                {
                    GoBack();
                }
            }
            if (e.Key.State == Key.StateType.Down && e.Key.KeyPressedName == "Return")
            {
                if (retryView != null)
                {
                    window.Remove(retryView);
                }
                if (retryAction != null) retryAction();
            }
        }
    }
}
