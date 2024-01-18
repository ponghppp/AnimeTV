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

namespace TVAnime.Helper
{
    internal class HttpHelper
    {
        public static async Task<HttpResponseMessage> MakeHttpRequest(string url, HttpMethod method, Dictionary<string, object> body = null, Dictionary<string, string> headers = null) 
        {
            HttpResponseMessage response = null;
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
                        if (type.ToString() == "application/x-www-form-urlencoded")
                        {
                            request.Content = new StringContent(data.ToString());
                            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        }
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
                        client.Timeout = TimeSpan.FromSeconds(30);
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
    }
}
