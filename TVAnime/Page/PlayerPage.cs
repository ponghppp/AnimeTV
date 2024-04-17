using ElmSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using TVAnime.Component;
using TVAnime.Helper;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class PlayerPage : BasePage
    {
        private Player player;
        private Content content;
        public override void Init()
        {
            content = new Content();
            var seriesId = GetSeriesId();
            player = new Player(this, seriesId, param["Title"].ToString(), param["Id"].ToString());
            content.view.Add(player.view);
            view.Add(content.view);
            GetVideo();
        }
        private async void GetVideo()
        {
            ShowLoading();
            loadingViewLabel.Text = "載入中...";
            player.shouldPlay = true;
            await Api.DownloadAnime(this, param["Id"].ToString(), param["ApiReq"].ToString(), loadingViewLabel);
            //await Task.Run(() => Api.CopyVideoFromResource("29109.mp4"));
            HideLoading();
            var id = param["Id"].ToString();
            var videoUrl = Constant.Download + "/" + id + ".mp4";
            var lastPlayTime = RecordHelper.GetVideoLastPlayTime(id);
            player.SetVideoSource(videoUrl, lastPlayTime);
        }
        private string GetSeriesId()
        {
            var apireq = HttpUtility.UrlDecode(param["ApiReq"].ToString());
            var json = JObject.Parse(apireq);
            var categoryId = json.Value<string>("c");
            return categoryId;
        }
        public void PreviousEpisode()
        {
            var series = (List<Episode>)param["Series"];
            var index = series.FindIndex(e => e.Id.ToString() == param["Id"].ToString());
            if (index > 0)
            {
                var nextIndex = Math.Min(series.Count - 1, index + 1);
                var episode = series[nextIndex];
                if (episode.Id.ToString() != param["Id"].ToString())
                {
                    client.CancelPendingRequests();
                    param["Id"] = episode.Id.ToString();
                    param["Title"] = episode.Title;
                    param["ApiReq"] = episode.ApiReq;
                    view.Remove(content.view);
                    Init();
                }
            }
        }
        public void NextEpisode()        
        {
            var series = (List<Episode>)param["Series"];
            var index = series.FindIndex(e => e.Id.ToString() == param["Id"].ToString());
            if (index > 0)
            {
                var nextIndex = Math.Max(0, index - 1);
                var episode = series[nextIndex];
                if (episode.Id.ToString() != param["Id"].ToString())
                {
                    client.CancelPendingRequests();
                    param["Id"] = episode.Id.ToString();
                    param["Title"] = episode.Title;
                    param["ApiReq"] = episode.ApiReq;
                    view.Remove(content.view);
                    Init();
                }
            }
        }
    }
}