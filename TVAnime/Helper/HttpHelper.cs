using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tizen.Pims.Contacts.ContactsViews;
using System.Net.WebSockets;
using TVAnime.Component;
using System.Net.Mime;
using System.IO;
using System.Net.NetworkInformation;
using Microsoft.VisualBasic;
using Tizen.NUI.BaseComponents;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Tizen.Network.IoTConnectivity;
using Tizen.Content.Download;

namespace TVAnime.Helper
{
    internal class HttpHelper
    {
        static bool connected = true;
        public static async Task<HttpResponseMessage> MakeHttpRequest(string url, HttpMethod method, Dictionary<string, object> body = null, Dictionary<string, string> headers = null)
        {
            if (!connected)
            {
                return GetMockData(url);
            }

            HttpResponseMessage response = null;
            HttpContent content = null;
            using (var request = new HttpRequestMessage())
            {
                try
                {
                    if (method == HttpMethod.Get && body != null)
                    {
                        url += "?";
                        foreach (var p in body)
                        {
                            url += (p.Key + "=" + p.Value);
                            if (p.Key != body.LastOrDefault().Key) url += "&";
                        }
                    }
                    request.Method = method;
                    request.RequestUri = new Uri(url);
                    if (method == HttpMethod.Post && body != null)
                    {
                        var data = body.GetValueOrDefault("data");
                        var type = body.GetValueOrDefault("type");
                        content = new StringContent(data.ToString());
                        content.Headers.ContentType = new MediaTypeHeaderValue(type.ToString());
                        request.Content = content;
                    }
                    if (headers != null)
                    {
                        foreach (var h in headers)
                        {
                            request.Headers.Add(h.Key, h.Value);
                        }
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(60);
                        response = await client.SendAsync(request);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return response;
        }

        public static async Task CheckNetworkConnectivity()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                    var response = await client.GetAsync("https://google.com");
                    connected = response.StatusCode == System.Net.HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static HttpResponseMessage GetMockData(string url)
        {
            var response = new HttpResponseMessage();
            var fileName = "";
            switch (url)
            {
                case "https://d1zquzjgwo9yb.cloudfront.net/":
                    fileName = "latestAnime";
                    break;
            }

            string text = File.ReadAllText(Path.Combine(Tizen.Applications.Application.Current.DirectoryInfo.Resource, fileName + ".txt"));
            response.Content = new StringContent(text);
            return response;
        }
        public static async Task DownloadFileTaskAsync(Uri uri, string FileName, Dictionary<string, string> headers)
        {
            using (var client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        client.DefaultRequestHeaders.Add(h.Key, h.Value);
                    }
                }
                using (var s = await client.GetStreamAsync(uri))
                {
                    using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
                    {
                        await s.CopyToAsync(fs);
                    }
                }
            }
        }
    }
}
