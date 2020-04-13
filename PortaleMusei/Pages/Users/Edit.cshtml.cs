using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages.Users
{
    [Authorize(Roles = "admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public UserWithRole EditingUser { get; set; }
        public bool LockedOut { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult OnGet(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound();
            }

            EditingUser = _userManager.Users.Where(u => u.Id == id).Select(u => new UserWithRole { User = u, Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() }).FirstOrDefault();

            if (EditingUser == null)
            {
                return NotFound();
            }

            LockedOut = EditingUser.User.LockoutEnd == null ? false : DateTime.Parse(EditingUser.User.LockoutEnd.ToString()) > DateTime.Now;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id, string role, bool lockout)
        {
            EditingUser = _userManager.Users.Where(u => u.Id == id).Select(u => new UserWithRole { User = u, Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault() }).FirstOrDefault();
            if (EditingUser == null)
            {
                return NotFound();
            }
            LockedOut = EditingUser.User.LockoutEnd == null ? false : DateTime.Parse(EditingUser.User.LockoutEnd.ToString()) > DateTime.Now;
            var userRoles = await _userManager.GetRolesAsync(EditingUser.User);
            await _userManager.RemoveFromRolesAsync(EditingUser.User, userRoles);
            if (role!="user" && _roleManager.RoleExistsAsync(role).Result)
            {
                await _userManager.AddToRoleAsync(EditingUser.User, role);
            }
            if (LockedOut != lockout)
            {
                if (lockout)
                {
                    await _userManager.SetLockoutEndDateAsync(EditingUser.User, new DateTimeOffset(2999, 1, 1, 0, 0, 0, new TimeSpan(2, 0, 0)));
                }
                else
                {
                    await _userManager.SetLockoutEndDateAsync(EditingUser.User, new DateTimeOffset(1970, 1, 1, 0, 0, 0, new TimeSpan(2, 0, 0)));
                }
            }
            return RedirectToPage("./Index");
        }

    }
}
