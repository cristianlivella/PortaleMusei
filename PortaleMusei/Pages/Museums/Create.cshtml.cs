using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Museums
{
    [Authorize(Roles = "staff,admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public Museum Museum { get; set; }
        [BindProperty]
        public List<Region> Regions { get; set;  }
        [BindProperty]
        public int SelectedRegionId { get; set; }
        private readonly PortaleMusei.Data.PortaleMuseiContext _context;

        public CreateModel(PortaleMusei.Data.PortaleMuseiContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Regions = _context.Regions.ToList();
            return Page();
        }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public IActionResult OnPost()
        {
            Regions = _context.Regions.ToList();
            Museum.Region = Regions.Where(r => r.Id == SelectedRegionId).FirstOrDefault();
            if (!ModelState.IsValid || Museum.Region==null)
            {
                return Page();
            }

            _context.Museums.Add(Museum);
            _context.SaveChanges();

            return RedirectToPage("./Details", new { id = Museum.Id });
        }
    }
}
