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
        public static Dictionary<string, object> GetCurrentPageParam()
        {
            var currentPage = Globals.pageStacks.LastOrDefault();
            if (currentPage != null)
            {
                return currentPage.Values.FirstOrDefault();
            }
            return null;
        }
        public static void UpdateCurrentPageParam(Dictionary<string, object> param)
        {
            var currentPage = Globals.pageStacks.LastOrDefault();
            if (currentPage != null)
            {
                var currentPageType = currentPage.Keys.FirstOrDefault();
                currentPage[currentPageType] = param;
            }
        }
    }
}
