using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PortaleMusei.Models
{
    public class Museum
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Region Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [Url]
        public string Website { get; set; }
        [Url]
        public string Picture { get; set; }
        [Url]
        public string Map { get; set; }

        public Museum()
        {
            City = Address = Website = Picture = Map = "";
        }
    }
}
