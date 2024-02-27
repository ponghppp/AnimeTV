using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tizen.Network.Connection;
using Tizen.NUI.BaseComponents;
using TVAnime.Page;

namespace TVAnime.Helper
{
    internal class HttpHelper
    {
        static bool connected = true;
        static TextLabel loadingViewLabel;
        public static async Task<HttpResponseMessage> MakeHttpRequest(BasePage page, string url, HttpMethod method, Dictionary<string, object> body = null, Dictionary<string, string> headers = null)
        {
            if (!connected)
            {
                return GetMockData(url, body);
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
                        page.client = client;
                        client.Timeout = TimeSpan.FromSeconds(60);
                        response = await client.SendAsync(request);
                        page.client = null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return response;
        }

        public static void CheckNetworkConnectivity()
        {
            //connected = false;
            //return;

            try
            {
                ConnectionItem connection = ConnectionManager.CurrentConnection;
                ConnectionState state = connection.State;
                if (state == ConnectionState.Connected)
                {
                    connected = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static HttpResponseMessage GetMockData(string url, Dictionary<string, object> body)
        {
            var response = new HttpResponseMessage();
            var fileName = "";
            if (url.Contains("https://d1zquzjgwo9yb.cloudfront.net/"))
            {
                fileName = "latestAnime";
            }
            if (body != null && body["cat"] != null)
            {
                fileName = "cat";
            }
            string text = File.ReadAllText(Path.Combine(Constant.Resource, fileName + ".txt"));
            response.Content = new StringContent(text);
            return response;
        }
        public static async Task DownloadFileTaskAsync(BasePage page, string url, string destinationFilePath, Dictionary<string, string> headers, TextLabel loadingLabel)
        {
            loadingViewLabel = loadingLabel;
            using (var client = new DownloadHelper(url, destinationFilePath, headers))
            {
                page.client = client._httpClient;
                client.ProgressChanged += ProgressChanged;
                await client.StartDownload();
                page.client = null;
            }
        }
        private static void ProgressChanged(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage)
        {
            loadingViewLabel.Text = "下載中..." + (int)progressPercentage + "%";
        }
    }
}
