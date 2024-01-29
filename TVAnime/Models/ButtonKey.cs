using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVAnime.Models
{
    internal class ButtonKey
    {
        public string Key { get; set; }
        public string Word { get; set; }
        public ButtonKey(string key, string word)
        {
            Key = key;
            Word = word;
        }   
    }
}
