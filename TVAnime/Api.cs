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
using Tizen.Applications;
using Tizen.Network.Connection;
using Tizen.Content.MediaContent;

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
                    var url = "https://v.anime1.me/api";
                    var body = new Dictionary<string, object>
                    {
                        ["data"] = values,
                        ["type"] = "application/x-www-form-urlencoded"
                    };
                    var response = await HttpHelper.MakeHttpRequest(url, HttpMethod.Post, body);
                    var setCookieHeaders = response.Headers.GetValues("Set-Cookie");
                    var downloadHeaders = new Dictionary<string, string>();
                    downloadHeaders["Cookie"] = "";
                    foreach (var header in setCookieHeaders)
                    {
                        var key = header.Substring(0, header.IndexOf('='));
                        var value = header.Substring(header.IndexOf('=') + 1, header.IndexOf(';') - 1);
                        downloadHeaders["Cookie"] += (key + "=" + value);
                    }
                    downloadHeaders["Range"] = "bytes=0-";
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    JObject json = JsonConvert.DeserializeObject<JObject>(jsonStr);
                    var downloadUrl = "https:" + json["s"][0].Value<string>("src");
                    callback = cb;

                    var dest = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads);
                    var dir = new System.IO.DirectoryInfo(dest);
                    foreach (var file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                    dest += "/anime.mp4";
                    var uri = new Uri(downloadUrl);
                    await HttpHelper.DownloadFileTaskAsync(uri, dest, downloadHeaders);

                    //var file = new File
                    //Application.Current.DirectoryInfo.Data
                    //var dest = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads);
                    //var type = Tizen.Content.Download.NetworkType.All;
                    //var req = new Tizen.Content.Download.Request(downloadUrl, dest, "anime.mp4", type, downloadHeaders);
                    //req.StateChanged += DownloadStateChanged;
                    //req.Start();

                    //callback(req.DestinationPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void Test()
        {
            var apireq = "%7B%22c%22%3A%221357%22%2C%22e%22%3A%222s%22%2C%22t%22%3A1706172809%2C%22p%22%3A0%2C%22s%22%3A%22ed5aaeae077e9969eee5c520f0d40e5d%22%7D";
            DownloadAnime(apireq, null);
        }
    }
}
