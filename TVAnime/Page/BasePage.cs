using Tizen.NUI.BaseComponents;
using Tizen.NUI;

namespace TVAnime.Page
{
    internal class BasePage
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
            view = v;
        }

        public void TransferToView(BasePage page) 
        {
            window.Add(page.view);
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                NUIApplication.Current.Exit();
            }
        }
    }
}
