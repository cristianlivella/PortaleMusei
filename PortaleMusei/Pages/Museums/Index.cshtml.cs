using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Museums
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PortaleMusei.Data.PortaleMuseiContext _context;
        public IList<Museum> Museums { get; set; }
        public int TotalMuseums { get; set; }
        public int CurrentPage { get; set; }
        public bool GridView { get; set; }
        public string SearchQuery { get; set; }

        public IndexModel(PortaleMusei.Data.PortaleMuseiContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int p = 1, string q = null, int grid = -1)
        {
            SearchQuery = q != null ? q : "";
            GridView = HttpContext.Session.GetInt32("grid_view").GetValueOrDefault(1) == 1;
            if (grid != -1)
            {
                GridView = grid == 1 ? true : false;
                HttpContext.Session.SetInt32("grid_view", GridView ? 1 : 0);
            }
            CurrentPage = p;
            Museums = await _context.Museums.Include(m => m.Region).ToListAsync();
            if (q != null)
            {
                Museums = Museums.Where(m => m.Name.ToLower().Contains(q.ToLower()) || m.City.ToLower().Contains(q.ToLower()) || m.Region.Name.ToLower().Contains(q.ToLower())).ToList();
            }
            TotalMuseums = Museums.Count();
            if (CurrentPage < 1 || CurrentPage > (TotalMuseums / 30 + (TotalMuseums % 30 > 0 ? 1 : 0)) && q==null)
            {
                return RedirectToPage("./Index");
            }
            if (TotalMuseums > 30)
            {
                Museums = Museums.Skip(30 * (CurrentPage - 1)).Take(30).ToList();
            }
            return Page();
        }
    }
}
