using TVAnime.Component;

namespace TVAnime.Page
{
    internal class PlayerPage : BasePage
    {
        private Player player;
        public override void Init()
        {
            var content = new Content();
            player = new Player(this, param["Title"].ToString(), param["Id"].ToString());
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
