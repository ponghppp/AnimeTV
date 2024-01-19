using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using System.Net.Http;
using Tizen.Pims.Contacts.ContactsViews;
using System.Net.Http.Headers;
using Tizen.Applications.Cion;
using TVAnime.Page;
using System.Collections.Generic;
using TVAnime.Models;
using ElmSharp;
using Tizen.NUI.Components;
using TVAnime.Helper;


namespace TVAnime
{
    internal class Program : NUIApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
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
