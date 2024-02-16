using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Web;
using TVAnime.Component;
using TVAnime.Models;

namespace TVAnime.Page
{
    internal class PlayerPage : BasePage
    {
        private Player player;
        public override void Init()
        {
            var content = new Content();
            var apireq = HttpUtility.UrlDecode(param["ApiReq"].ToString());
            var json = JObject.Parse(apireq);
            var categoryId = json.Value<string>("c");
            player = new Player(this, categoryId, param["Title"].ToString(), param["Id"].ToString());
            content.view.Add(player.view);
            view.Add(content.view);
            GetVideo();
        }

        private async void GetVideo()
        {
            ShowLoading();
            await Api.DownloadAnime(this, param["Id"].ToString(), param["ApiReq"].ToString(), loadingViewLabel);
            //await Task.Run(() => Api.CopyVideoFromResource("29109.mp4"));
            HideLoading();
            var videoUrl = Constant.Download + "/" + param["Id"].ToString() + ".mp4";
            player.SetVideoSource(videoUrl);
        }

    }
}
