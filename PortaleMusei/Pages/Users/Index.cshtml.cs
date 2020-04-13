using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Users
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;        
        public IEnumerable<UserWithRole> Users { get; set; }
        public string CurrentUserId { get; set; }
        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGetAsync()
        {
            CurrentUserId = _userManager.GetUserId(User);
            Users = _userManager.Users.Select(u => new UserWithRole { User = u, Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() });
        }
    }
}
