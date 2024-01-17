using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using System.Net.Http;
using Tizen.Pims.Contacts.ContactsViews;
using System.Net.Http.Headers;
using Tizen.Applications.Cion;


namespace TVAnime
{
    internal class Program : NUIApplication
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        async void GetAnimeUrlAsync()
        {

            using (var request = new HttpRequestMessage())
            {
                try
                {

                    var values = "d=%7B%22c%22%3A%221357%22%2C%22e%22%3A%222s%22%2C%22t%22%3A1704252343%2C%22p%22%3A0%2C%22s%22%3A%221b70eb14fcd349a38fa55a81c965e2fe%22%7D";
                    var content = new StringContent(values);
                    var url = "https://v.anime1.me/api";
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(url);
                    request.Content = content;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(10);
                        var response = await client.SendAsync(request);
                        string responseBody = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        void Initialize()
        {
            var window = Window.Instance;
            Window.Instance.KeyEvent += OnKeyEvent;

            //window.Add(webView);

            View view = new View()
            {
                HeightResizePolicy = ResizePolicyType.FillToParent,
                WidthResizePolicy = ResizePolicyType.FillToParent
            };

            view.Add(webView);
            window.Add(view);

            GetAnimeUrlAsync();

        }
        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down && (e.Key.KeyPressedName == "XF86Back" || e.Key.KeyPressedName == "Escape"))
            {
                Exit();
            }
        }

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
