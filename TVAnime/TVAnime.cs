using System.IO;
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
            //DeleteEverything();
            VideoHelper.DeleteOldVideos();
            Initialize();

        }

        void DeleteEverything()
        {
            var dirInfo = new DirectoryInfo(Constant.Download);
            foreach (var f in dirInfo.GetFiles())
            {
                f.Delete();
            }
        }

        void Initialize()
        {
            //var param = new Dictionary<string, object>()
            //{
            //    ["Id"] = "29109",
            //    ["Title"] = "Example"
            //};
            //var playerPage = new PlayerPage();
            //playerPage.param = param;
            //Tizen.NUI.Window.Instance.Add(playerPage.view);
            //playerPage.Init();
            //Globals.AddPageStack(playerPage.GetType(), null);

            //var searchPage = new SearchPage();
            //Tizen.NUI.Window.Instance.Add(searchPage.view);
            //searchPage.Init();
            //Globals.AddPageStack(searchPage.GetType(), null);
            //return;

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
