using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVAnime.Helper
{
    internal class TimeHelper
    {
        public static string MillisecondsToMinute(int milliseconds)
        {
            var seconds = milliseconds / 1000;
            var minutes = seconds / 60;
            var s = seconds % 60;
            return minutes.ToString("D2") + ":" + s.ToString("D2");
        }
    }
}
