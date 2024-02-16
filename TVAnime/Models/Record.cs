using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVAnime.Models
{
    internal class Record
    {
        public string CategoryId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public int PlayTime { get; set; }
        public int Duration { get; set; }
    }
}
