using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using System.Collections.Generic;
using TVAnime.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Runtime.CompilerServices;
using System.Linq;

namespace TVAnime.Page
{
    internal abstract class BasePage
    {
        List<EventHandler<Window.KeyEventArgs>> delegates = new List<EventHandler<Window.KeyEventArgs>>();
        public View view { get; set; }
        public View loadingView { get; set; }
        public Window window = Window.Instance;
        public Dictionary<string, object> param { get; set; }

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

        public void TransferToView(Type pageType, Dictionary<string, object> param = null, bool addStack = true)
        {
            if (Globals.pageStacks.Count >= 2)
            {
                var previousPageType = Globals.pageStacks[Globals.pageStacks.Count - 2];
                if (pageType == previousPageType.Keys.FirstOrDefault())
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
                    Opacity = 0.6f
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
                TextLabel textLabel = new TextLabel()
                {
                    Text = "載入中...",
                    TextColor = Color.White,
                };
                View spinner = new View()
                {
                    WidthSpecification = 100,
                    HeightSpecification = 100,
                    BackgroundColor = Color.Cyan
                };
                var animation = new Animation(1000)
                {
                    Looping = true
                };
                animation.AnimateTo(spinner, "Orientation", new Rotation(new Radian(new Degree(180.0f)), PositionAxis.X), 0, 500, new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseInOutSine));
                animation.Play();

                container.Add(spinner);
                container.Add(textLabel);
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
            TransferToView(previousPageType.Keys.FirstOrDefault(), previousPageType.Values.FirstOrDefault(), false);
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
