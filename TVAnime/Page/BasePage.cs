using Tizen.NUI.BaseComponents;
using Tizen.NUI;
using Tizen.NUI.Components;
using System.Collections.Generic;
using TVAnime.Models;
using System.Runtime.InteropServices.WindowsRuntime;

namespace TVAnime.Page
{
    internal abstract class BasePage
    {
        public View view { get; set; }
        public Window window = Window.Instance;

        public BasePage()
        {
            window.KeyEvent += OnKeyEvent;
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

        public virtual void Init()
        {
            Globals.currentPage = this;
            window.Add(view);
        }

        public void TransferToView(BasePage page)
        {
            Layer layer = new Layer();
            window.AddLayer(layer);
            layer.Add(page.view);
            window.AddLayer(layer);
            Globals.pageCount += 1;
            Globals.currentPage = page;
        }

        public virtual void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                if (Globals.pageCount == 1)
                {
                    NUIApplication.Current.Exit();
                }
                if (this == Globals.currentPage)
                {
                    Globals.pageCount -= 1;
                    window.RemoveLayer(this.view.GetLayer());
                }
            }
        }
    }
}
