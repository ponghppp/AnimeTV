using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.NUI.BaseComponents;
using TVAnime.Component;

namespace TVAnime.Page
{
    internal class PlayerPage: BasePage
    {
        public override void Init()
        {
            var content = new Content();
            var player = new Player();
            content.view.Add(player.view);
            view.Add(content.view);
            GetVideo();
        }

        private void GetVideo()
        {
            ShowLoading();
            Api.DownloadAnime(param["ApiReq"].ToString(), SetupPlayer);
            
        }

        private void SetupPlayer(string downloadPath)
        {
            HideLoading();
            Console.WriteLine(downloadPath);
        }
    }
}
