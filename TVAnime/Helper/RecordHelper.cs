using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.System;
using TVAnime.Models;

namespace TVAnime.Helper
{
    internal class RecordHelper
    {
        public static string videoRecordPath = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads) + "/video.txt";
        public static string recordJsonPath = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads) + "/record.json";
        public static void RecordCurrentVideo(string id)
        {
            File.CreateText(videoRecordPath).Close();
            File.WriteAllText(videoRecordPath, id);
        }
        public static string GetCurrentVideo()
        {
            var id = "";
            using (StreamReader r = new StreamReader(recordJsonPath))
            {
                id = r.ReadToEnd();
            }
            return id;
        }
        public static void RecordVideoPlayTime(string id, string title, int playTime, int duration)
        {
            if (!File.Exists(recordJsonPath))
            {
                File.Create(recordJsonPath).Close();
            }
            List<Record> data = new List<Record>();
            using (StreamReader r = new StreamReader(recordJsonPath))
            {
                string j = r.ReadToEnd();
                if (j != "") data = JsonConvert.DeserializeObject<List<Record>>(j);
            }
            var rec = data.FirstOrDefault(d => d.id == id);
            if (rec == null)
            {
                data.Add(new Record()
                {
                    id = id,
                    title = title,
                    playTime = playTime,
                    duration = duration
                });
            } 
            else
            {
                rec.playTime = playTime;
            }
            string json = JsonConvert.SerializeObject(data.ToArray());
            File.WriteAllText(recordJsonPath, json);
        }
        public static List<Record> GetAllRecords()
        {
            if (!File.Exists(recordJsonPath))
            {
                File.Create(recordJsonPath).Close();
            }
            List<Record> data = new List<Record>();
            using (StreamReader r = new StreamReader(recordJsonPath))
            {
                string j = r.ReadToEnd();
                if (j != "") data = JsonConvert.DeserializeObject<List<Record>>(j);
            }
            return data;
        }
    }
}
