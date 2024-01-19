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
                page.Init();
                page.param = param;
                window.Add(page.view);
                if (addStack)
                {
                    Globals.AddPageStack(pageType, param);
                }
            }
        }

        public void ShowLoading()
        {
            Layer layer = new Layer();
            View view = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };
            layer.Add(view);
            window.AddLayer(layer);
        }

        public void HideLoading()
        {
            var loadingLayer = window.GetLayer(window.LayerCount);
            window.RemoveLayer(loadingLayer);
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
