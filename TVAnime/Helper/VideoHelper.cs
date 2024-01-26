using Tizen.Content.MediaContent;

namespace TVAnime.Helper
{
    internal class VideoHelper
    {
        public static int GetVideoDuration(string videoName)
        {
            int duration = 0;
            var mediaDatabase = new MediaDatabase();
            mediaDatabase.Connect();

            var folderCmd = new FolderCommand(mediaDatabase);
            var folderSelectArguments = new SelectArguments()
            {
                FilterExpression = $"{FolderColumns.Name}='Downloads'"
            };

            using (var mediaDataReader = folderCmd.Select(folderSelectArguments))
            {
                var mediaSelectArguments = new SelectArguments()
                {
                    FilterExpression = $"{MediaInfoColumns.MediaType}={(int)Media​Type.Video} AND {MediaInfoColumns.DisplayName}={videoName}",
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
