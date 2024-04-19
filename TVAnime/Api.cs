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
using Tizen.NUI.BaseComponents;
using TVAnime.Page;
using System.IO;
using Tizen.NUI.Components;

namespace TVAnime
{
    internal class Api
    {
        public static async Task<List<LatestAnime>> GetLatestList(BasePage page)
        {
            try
            {
                var url = "https://d1zquzjgwo9yb.cloudfront.net/";
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Get);
                if (response == null) return new List<LatestAnime> { };

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

        public static async Task<List<Episode>> GetSeries(BasePage page, string url)
        {
            try
            {
                List<Episode> episodeList = new List<Episode>() { };
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Get);
                if (response == null) return new List<Episode> { };

                var html = await response.Content.ReadAsStringAsync();
                if (html.Contains("nav-previous"))
                {
                    var divs = HtmlHelper.GetTags(html, "div", "class=nav-previous");
                    var firstA = HtmlHelper.GetInnerHtml(divs[0]);
                    var otherPageUrl = HtmlHelper.GetNodeAttribute(firstA, "href");
                    episodeList = await GetSeries(page, otherPageUrl);
                }

                var h2s = HtmlHelper.GetTags(html, "h2", "class=entry-title").Select(h => HtmlHelper.GetInnerHtml(h));
                var animeNames = h2s.Select(a => HtmlHelper.GetInnerHtml(a)).ToList();
                var hrefs = h2s.Select(a => HtmlHelper.GetNodeAttribute(a, "href"));

                var animeIds = hrefs.Select(h => h.Substring(h.LastIndexOf('/') + 1)).ToList();
                animeIds.RemoveAll(a => a == "");
                var videos = HtmlHelper.GetTags(html, "video");
                var apireqs = videos.Select(v => HtmlHelper.GetNodeAttribute(v, "data-apireq")).ToList();

                for (var i = 0; i < animeIds.Count(); i++)
                {
                    episodeList.Add(new Episode()
                    {
                        Id = int.Parse(animeIds[i]),
                        Title = animeNames[i],
                        ApiReq = apireqs[i]
                    });
                }
                return episodeList.OrderByDescending(e => e.Id).ToList();
            }
            catch (Exception ex)
            {
                return new List<Episode>() { };
            }
        }
        public static async Task<List<Episode>> GetSeriesByEpisode(BasePage page, string episodeId)
        {
            try
            {
                var url = "https://anime1.me/" + episodeId;
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Get);
                if (response == null) return new List<Episode> { };

                var html = await response.Content.ReadAsStringAsync();
                var searchText = "cat=";
                var searchTextIndex = html.IndexOf(searchText);
                var firstQuote = html.IndexOf("\"", searchTextIndex);
                var catId = html.Substring(searchTextIndex + searchText.Length, firstQuote - searchTextIndex - searchText.Length);
                var seriesUrl = "https://anime1.me?cat=" + catId;
                var episodeList = await GetSeries(page, seriesUrl);
                return episodeList;
            }
            catch (Exception ex)
            {
                return new List<Episode>() { };
            }
        }

        public static async Task DownloadAnime(BasePage page, string id, string apireq, TextLabel loadingLabel)
        {
            try
            {
                var currentVideos = VideoHelper.GetCurrentVideos();
                if (currentVideos.Contains(id))
                {
                    return;
                }

                var values = "d=" + apireq;
                var url = "https://v.anime1.me/api";
                var body = new Dictionary<string, object>
                {
                    ["data"] = values,
                    ["type"] = "application/x-www-form-urlencoded"
                };
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Post, body);
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

                var dest = Constant.Download + "/" + id + ".mp4";
                await HttpHelper.DownloadFileTaskAsync(page, downloadUrl, dest, downloadHeaders, loadingLabel);
                VideoHelper.DeleteOldVideos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void CopyVideoFromResource(string resourceFileName)
        {
            var resPath = Constant.Resource + resourceFileName;
            var destPath = Constant.Download + "/" + resourceFileName;
            byte[] bytes = File.ReadAllBytes(resPath);
            File.WriteAllBytes(destPath, bytes);
        }

        public static async Task<List<Episode>> GetSeason(BasePage page, string season)
        {
            try
            {
                var url = "https://anime1.me/" + season;
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Get);
                if (response == null) return new List<Episode> { };

                var html = await response.Content.ReadAsStringAsync();
                var tagAs = HtmlHelper.GetTags(html, "a", "href^=\"/?cat=\"");
                tagAs = tagAs.Where(a => !a.Contains("anime1.pw")).ToList();
                var episodeList = tagAs.Select(a =>
                {
                    return new Episode()
                    {
                        Id = int.Parse(HtmlHelper.GetNodeAttribute(a, "href").Split("=").LastOrDefault()),
                        Title = HtmlHelper.GetInnerHtml(a),
                    };
                }).ToList();
                return episodeList;
            }
            catch (Exception ex)
            {
                return new List<Episode>() { };
            }
        }

        public static async Task<List<Category>> GetSearchResult(BasePage page, string searchText, int resultPage = 1, int times = 1)
        {
            try
            {
                List<Category> categories = new List<Category>() { };
                var url = $"https://anime1.me/page/{resultPage}?s={searchText}";
                var response = await HttpHelper.MakeHttpRequest(page, url, HttpMethod.Get);
                if (response == null) return new List<Category> { };

                var html = await response.Content.ReadAsStringAsync();

                if (html.Contains("nav-previous") && times <= 5)
                {
                    categories = await GetSearchResult(page, searchText, resultPage + 1, times + 1);
                }

                var articles = HtmlHelper.GetTags(html, "article");
                var cats = articles.Select(a =>
                {
                    var footer = HtmlHelper.GetTags(a, "footer").FirstOrDefault();
                    var span = HtmlHelper.GetTags(footer, "span", "class=\"cat-links\"").FirstOrDefault();
                    var firstA = HtmlHelper.GetTags(span, "a").FirstOrDefault();
                    var title = HtmlHelper.GetInnerHtml(firstA);
                    var id = HtmlHelper.GetNodeAttribute(firstA, "href").Replace("https://anime1.me/category/", "");
                    return new Category(id, title);
                }).GroupBy(c => c.Id).Select(c => c.FirstOrDefault());

                categories.AddRange(cats);
                return categories.GroupBy(c => c.Id).Select(c => c.FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                return new List<Category>() { };
            }
        }
    }
}
