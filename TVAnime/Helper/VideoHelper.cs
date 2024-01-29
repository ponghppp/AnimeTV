using System.Linq;
using System.Threading.Tasks;
using Tizen.Content.MediaContent;

namespace TVAnime.Helper
{
    internal class VideoHelper
    {
        public static void DeleteAllVideos()
        {
            var dir = new System.IO.DirectoryInfo(Constant.Download);
            foreach (var file in dir.GetFiles("*.mp4"))
            {
                file.Delete();
            }
        }
        public static void DeleteOldVideos(int maxCount = 5)
        {
            var dir = new System.IO.DirectoryInfo(Constant.Download);
            var files = dir.GetFiles("*.mp4").OrderBy(v => v.CreationTime).ToList();
            if (files.Count > maxCount) files.RemoveRange(0, files.Count - maxCount);
        }

        public static async Task<int> GetVideoDuration(string videoName)
        {
            int duration = 0;
            var mediaDatabase = new MediaDatabase();
            mediaDatabase.Connect();
            await mediaDatabase.ScanFolderAsync(Constant.Download, true);

            var folderCmd = new FolderCommand(mediaDatabase);
            var folderSelectArguments = new SelectArguments()
            {
                FilterExpression = $"{FolderColumns.Name}='Downloads'"
            };
            using (var mediaDataReader = folderCmd.Select(folderSelectArguments))
            {
                var mediaSelectArguments = new SelectArguments()
                {
                    FilterExpression = $"{MediaInfoColumns.MediaType}={(int)Media​Type.Video} AND {MediaInfoColumns.DisplayName}='{videoName}'",
                };
                while (mediaDataReader.Read())
                {
                    var folder = mediaDataReader.Current;
                    var media = folderCmd.SelectMedia(folder.Id, mediaSelectArguments);
                    while (media.Read())
                    {
                        var mediaInfo = media.Current;
                        switch (mediaInfo.MediaType)
                        {
                            case MediaType.Video:
                                VideoInfo videoInfo = mediaInfo as VideoInfo;
                                var vName = videoInfo.DisplayName;
                                duration = videoInfo.Duration;
                                break;
                        }
                    }
                }
            }
            mediaDatabase.Disconnect();
            return duration;
        }
    }
}
