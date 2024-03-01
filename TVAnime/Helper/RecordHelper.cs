using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TVAnime.Models;

namespace TVAnime.Helper
{
    internal class RecordHelper
    {
        public static string videoRecordPath = Constant.Download + "/video.txt";
        public static string recordJsonPath = Constant.Download + "/record.json";
        public static void RecordCurrentVideo(string id)
        {
            try
            {
                File.CreateText(videoRecordPath).Close();
                File.WriteAllText(videoRecordPath, id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string GetCurrentVideo()
        {
            var id = "";
            if (!File.Exists(videoRecordPath))
            {
                File.Create(videoRecordPath).Close();
            }
            using (StreamReader r = new StreamReader(videoRecordPath))
            {
                id = r.ReadToEnd();
            }
            return id;
        }

        public static void RecordVideoPlayTime(string categoryId, string id, string title, int playTime, int duration)
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
            var rec = data.FirstOrDefault(d => d.Id == id);
            if (rec == null)
            {
                data.Add(new Record()
                {
                    CategoryId = categoryId,
                    Id = id,
                    Title = title,
                    PlayTime = playTime,
                    Duration = duration
                });
            }
            else
            {
                rec.PlayTime = playTime;
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
        public static int GetVideoLastPlayTime(string videoId)
        {
            if (!File.Exists(recordJsonPath))
            {
                File.Create(recordJsonPath).Close();
                return 0;
            }
            List<Record> data = new List<Record>();
            using (StreamReader r = new StreamReader(recordJsonPath))
            {
                string j = r.ReadToEnd();
                if (j != "") data = JsonConvert.DeserializeObject<List<Record>>(j);
            }
            var record = data.FirstOrDefault(d => d.Id == videoId);
            if (record == null) return 0;
            return record.PlayTime;
        }
    }
}