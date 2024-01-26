using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVAnime.Page;

namespace TVAnime.Models
{
    static class Globals
    {
        public static List<Dictionary<Type, Dictionary<string, object>>> pageStacks;

        public static void AddPageStack(Type pageType, Dictionary<string, object> param)
        {
            if (Globals.pageStacks == null)
            {
                Globals.pageStacks = new List<Dictionary<Type, Dictionary<string, object>>> { };
            }
            Dictionary<Type, Dictionary<string, object>> dict = new Dictionary<Type, Dictionary<string, object>> { };
            dict[pageType] = param;
            Globals.pageStacks.Add(dict);
        }
    }
}
