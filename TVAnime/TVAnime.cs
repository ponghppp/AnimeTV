using System.Threading;
using Tizen.NUI;
using TVAnime.Helper;
using TVAnime.Models;
using TVAnime.Page;


namespace TVAnime
{
    internal class Program : NUIApplication
    {
        protected override async void OnCreate()
        {
            base.OnCreate();
            //await HttpHelper.CheckNetworkConnectivity();
            Initialize();
        }

        void Initialize()
        {
            Thread.Sleep(2000);
            Api.Test();
            return;
            var homePage = new HomePage();
            Tizen.NUI.Window.Instance.Add(homePage.view);
            homePage.Init();
            Globals.AddPageStack(homePage.GetType(), null);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
