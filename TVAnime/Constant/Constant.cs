using System.Collections.Generic;
using System.Linq;
using Tizen.System;
using TVAnime.Models;

internal class Constant
{
    public static string Download = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads);
    public static string Resource = Tizen.Applications.Application.Current.DirectoryInfo.Resource;

    public static List<List<ButtonKey>> buttonKeys = new List<List<ButtonKey>>()
    {
        new List<ButtonKey> {
            new ButtonKey("1", "1"),
            new ButtonKey("2", "2"),
            new ButtonKey("3", "3"),
            new ButtonKey("4", "4"),
            new ButtonKey("5", "5"),
            new ButtonKey("6", "6"),
            new ButtonKey("7", "7"),
            new ButtonKey("8", "8"),
            new ButtonKey("9", "9"),
            new ButtonKey("0", "0"),
            },
         new List<ButtonKey>
         {
            new ButtonKey("q", "手"),
            new ButtonKey("w", "田"),
            new ButtonKey("e", "水"),
            new ButtonKey("r", "口"),
            new ButtonKey("t", "廿"),
            new ButtonKey("y", "卜"),
            new ButtonKey("u", "山"),
            new ButtonKey("i", "戈"),
            new ButtonKey("o", "人"),
            new ButtonKey("p", "心"),
         },
         new List<ButtonKey>
         {
            new ButtonKey("a", "日"),
            new ButtonKey("s", "尸"),
            new ButtonKey("d", "木"),
            new ButtonKey("f", "火"),
            new ButtonKey("g", "土"),
            new ButtonKey("h", "竹"),
            new ButtonKey("j", "十"),
            new ButtonKey("k", "大"),
            new ButtonKey("l", "中"),
            new ButtonKey("delete", "刪除"),
         },
        new List<ButtonKey>
         {
            new ButtonKey("z", "重"),
            new ButtonKey("x", "難"),
            new ButtonKey("c", "金"),
            new ButtonKey("v", "女"),
            new ButtonKey("b", "月"),
            new ButtonKey("n", "弓"),
            new ButtonKey("m", "一"),
            new ButtonKey("search", "搜尋"),
         }
    };
}

