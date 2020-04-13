using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortaleMusei.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public long Timestamp { get; set; }
        public string Picture { get; set; }
        public bool Read { get; set; }
    }
}
