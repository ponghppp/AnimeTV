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
            homePage.Init();
            Globals.pageCount = 1;
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
