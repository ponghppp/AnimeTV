﻿using System.Collections.Generic;
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
            DeleteOldVideos();
            Initialize();

        }
        void DeleteOldVideos()
        {
            var dirInfo = new DirectoryInfo(Constant.Download);
            foreach (var f in dirInfo.GetFiles("*.mp4").OrderBy(f => f.CreationTime).Take(5))
            {
                f.Delete();
            }
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
