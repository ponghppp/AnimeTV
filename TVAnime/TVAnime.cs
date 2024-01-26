using System.Threading;
using Tizen.NUI;
using TVAnime.Helper;
using TVAnime.Models;
using TVAnime.Page;


namespace TVAnime
{
    internal class Program : NUIApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            HttpHelper.CheckNetworkConnectivity();
            Initialize();
        }

        void Initialize()
        {
            //var playerPage = new PlayerPage();
            //Tizen.NUI.Window.Instance.Add(playerPage.view);
            //playerPage.Init();
            //Globals.AddPageStack(playerPage.GetType(), null);

            VideoHelper.GetVideoDuration("anime.mp4");
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
