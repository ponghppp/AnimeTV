using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TVAnime.Helper;
using Newtonsoft.Json;
using TVAnime.Models;
using Newtonsoft.Json.Linq;
using Tizen.System;

namespace TVAnime
{
    internal class Api
    {
        public static Action<string> callback;
        public static async Task<List<LatestAnime>> GetLatestList()
        {
            try
            {
                var url = "https://d1zquzjgwo9yb.cloudfront.net/";
                var response = await HttpHelper.MakeHttpRequest(url, HttpMethod.Get);
                var jsonStr = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<string[][]>(jsonStr);
                List<LatestAnime> animeList = list.Select(l => new LatestAnime()
                {
                    categoryId = int.Parse(l[0]),
                    animeName = l[1],
                    episode = l[2],
                    year = l[3],
                    season = l[4]
                }).Where(l => !(l.animeName.Contains("https://anime1.pw"))).ToList();
                return animeList;
            }
            catch (Exception ex)
            {
                return new List<LatestAnime>() { };
            }
        }

        public static async Task<List<Episode>> GetSeries(string seriesId)
        {
            try
            {
                var url = "https://anime1.me/";
                var body = new Dictionary<string, object>
                {
                    ["cat"] = seriesId
                };
                var response = await HttpHelper.MakeHttpRequest(url, HttpMethod.Get, body);
                var html = await response.Content.ReadAsStringAsync();

                var h2s = HtmlHelper.GetTags(html, "h2").Select(h => HtmlHelper.GetInnerHtml(h));
                var animeNames = h2s.Select(a => HtmlHelper.GetInnerHtml(a)).ToList();
                var hrefs = h2s.Select(a => HtmlHelper.GetNodeAttribute(a, "href"));

                var animeIds = hrefs.Select(h => h.Substring(h.LastIndexOf('/') + 1)).ToList();
                var videos = HtmlHelper.GetTags(html, "video");
                var apireqs = videos.Select(v => HtmlHelper.GetNodeAttribute(v, "data-apireq")).ToList();
                List<Episode> episodeList = new List<Episode>() { };
                for (var i = 0; i < animeIds.Count(); i++)
                {
                    episodeList.Add(new Episode()
                    {
                        Id = int.Parse(animeIds[i]),
                        Title = animeNames[i],
                        ApiReq = apireqs[i]
                    });
                }
                return episodeList;
            }
            catch (Exception ex)
            {
                return new List<Episode>() { };
            }
        }

        public static async void DownloadAnime(string apireq, Action<string> cb)
        {
            using (var request = new HttpRequestMessage())
            {
                try
                {
                    var values = "d=" + apireq;
                    //var content = new StringContent(values);
                    var url = "https://v.anime1.me/api";
                    var body = new Dictionary<string, object>
                    {
                        ["data"] = values,
                        ["type"] = "application/x-www-form-urlencoded"
                    };
                    var response = await HttpHelper.MakeHttpRequest(url, HttpMethod.Post, body);
                    var setCookieHeaders = response.Headers.GetValues("Set-Cookie");
                    var downloadHeaders = new Dictionary<string, string>();
                    foreach (var header in setCookieHeaders)
                    {
                        var key = header.Substring(0, header.IndexOf('='));
                        var value = header.Substring(header.IndexOf('=') + 1, header.IndexOf(';'));
                        downloadHeaders[key] = value;
                    }
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    JObject json = JsonConvert.DeserializeObject<JObject>(jsonStr);
                    var downloadUrl = json["s"][0].Value<string>("src");
                    var dest = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads) + "/anime.mp4";
                    //var req = new Tizen.Content.Download.Request("https://google.com");
                    //var dest = "/opt/usr/home/owner/content/Downloads/";
                    var req = new Tizen.Content.Download.Request(downloadUrl, dest, "anime.mp4", Tizen.Content.Download.NetworkType.All, downloadHeaders);
                    req.StateChanged += DownloadStateChanged;
                    req.Start();
                    callback = cb;
                    //callback(req.DestinationPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void DownloadStateChanged(object sender, Tizen.Content.Download.StateChangedEventArgs e)
        {
            if (e.State == Tizen.Content.Download.DownloadState.Completed)
            {
                callback(((Tizen.Content.Download.Request)sender).DestinationPath);
            }
        }
    }
}
