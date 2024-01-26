using System.IO;
using System.Linq;
using System.Threading;
using Tizen.NUI;
using Tizen.System;
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
            Initialize();
            
        }

        void DeleteEverything()
        {
            var dirInfo = new DirectoryInfo(StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads));
            foreach (var f in dirInfo.GetFiles())
            {
                f.Delete();
            }
        }

        void Initialize()
        {
            //var playerPage = new PlayerPage();
            //Tizen.NUI.Window.Instance.Add(playerPage.view);
            //playerPage.Init();
            //Globals.AddPageStack(playerPage.GetType(), null);
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
