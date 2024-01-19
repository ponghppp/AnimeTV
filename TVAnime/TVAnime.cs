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
           // Globals.pageStacks = new List<Dictionary<Type, Dictionary<string, object>>> { };
            var homePage = new HomePage();
            homePage.Init();
            Tizen.NUI.Window.Instance.Add(homePage.view);
            Globals.AddPageStack(homePage.GetType(), null);
            //Dictionary<Type, Dictionary<string, object>> dict = new Dictionary<Type, Dictionary<string, object>> { };
            //dict[homePage.GetType()] = null;
            //Globals.pageStacks.Add(dict);
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
