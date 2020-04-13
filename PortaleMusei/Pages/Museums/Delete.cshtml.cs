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
    [Authorize(Roles = "staff,admin")]
    public class DeleteModel : PageModel
    {
        private readonly PortaleMusei.Data.PortaleMuseiContext _context;

        public DeleteModel(PortaleMusei.Data.PortaleMuseiContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Museum = await _context.Museums.FindAsync(id);

            if (Museum != null)
            {
                _context.Museums.Remove(Museum);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
