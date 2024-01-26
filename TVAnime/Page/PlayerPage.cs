using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using Tizen.System;
using TVAnime.Component;

namespace TVAnime.Page
{
    internal class PlayerPage: BasePage
    {
        private Player player;
        public override void Init()
        {
            var content = new Content();
            player = new Player(this, param["Title"].ToString());
            content.view.Add(player.view);
            view.Add(content.view);
            GetVideo();
        }

        private async void GetVideo()
        {
            ShowLoading();
            await Api.DownloadAnime(this, param["ApiReq"].ToString(), loadingViewLabel);
            HideLoading();
            var videoUrl = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads) + "/anime.mp4";
            player.videoView.ResourceUrl = videoUrl;
            player.isPlaying = true;
            player.videoView.Play();
        }

    }
}
