using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Models;

namespace PortaleMusei.Data
{
    public class PortaleMuseiContext : DbContext
    {
        public PortaleMuseiContext (DbContextOptions<PortaleMuseiContext> options)
            : base(options)
        {
        }

        public DbSet<PortaleMusei.Models.Museum> Museums { get; set; }
        public DbSet<PortaleMusei.Models.Region> Regions { get; set; }
    }
}
