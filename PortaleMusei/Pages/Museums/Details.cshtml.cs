using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Museums
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PortaleMusei.Data.PortaleMuseiContext _context;

        public DetailsModel(PortaleMusei.Data.PortaleMuseiContext context)
        {
            _context = context;
        }

        public Museum Museum { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Museum = await _context.Museums.Include(m => m.Region).FirstOrDefaultAsync(m => m.Id == id);

            if (Museum == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
