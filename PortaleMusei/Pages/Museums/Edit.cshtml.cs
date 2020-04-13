using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Museums
{
    [Authorize(Roles = "staff,admin")]
    public class EditModel : PageModel
    {
        private readonly PortaleMusei.Data.PortaleMuseiContext _context;

        public EditModel(PortaleMusei.Data.PortaleMuseiContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Museum Museum { get; set; }
        public List<Region> Regions { get; set; }
        [BindProperty]
        public int SelectedRegionId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Museum = await _context.Museums.FirstOrDefaultAsync(m => m.Id == id);
            Regions = _context.Regions.ToList();
            SelectedRegionId = Museum.Region.Id;

            if (Museum == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Regions = _context.Regions.ToList();
            Museum.Region = Regions.Where(r => r.Id == SelectedRegionId).FirstOrDefault();
            _context.Attach(Museum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MuseumExists(Museum.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./Details", new { id = Museum.Id });
        }

        private bool MuseumExists(int id)
        {
            return _context.Museums.Any(e => e.Id == id);
        }
    }
}
