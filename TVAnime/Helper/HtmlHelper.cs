using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace TVAnime.Helper
{
    internal class HtmlHelper
    {
        public static List<string> GetTags(string html, string tag)
        {
            List<string> tags = new List<string>();
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            tags = htmlDocument.DocumentNode.QuerySelectorAll(tag).Select(x => x.OuterHtml).ToList();
            return tags;
        }
        public static string GetInnerHtml(string html)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument.DocumentNode.FirstChild.InnerHtml;
        }
        public static string GetNodeAttribute(string nodeHtml, string attr)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(nodeHtml);
            return htmlDocument.DocumentNode.FirstChild.Attributes[attr].Value;
        }
    }
}
