using System.Linq;
using Tizen.System;

internal class Constant
{
    public static string Download = StorageManager.Storages.FirstOrDefault().GetAbsolutePath(DirectoryType.Downloads);
    public static string Resource = Tizen.Applications.Application.Current.DirectoryInfo.Resource;
}

