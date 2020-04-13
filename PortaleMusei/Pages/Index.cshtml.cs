using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PortaleMusei.Data;
using PortaleMusei.Models;

namespace PortaleMusei.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PortaleMuseiContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public string NewsHeader { get; set; }
        public IEnumerable<News> NewsToShow { get; set; }
        public IEnumerable<Museum> CarouselMuseums { get; set; }
        public IndexModel(ILogger<IndexModel> logger, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, PortaleMuseiContext context)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGet()
        {
            if (_signInManager.IsSignedIn(User))
            {
                NewsHeader = "Notizie non lette";
                NewsToShow = await MuseiNewsApi.GetUnreadNews(_userManager.GetUserId(User));
            }
            if (NewsToShow == null || NewsToShow.Count() == 0) {
                NewsHeader = "Ultime notizie";
                NewsToShow = await MuseiNewsApi.GetLastNews();
            }
            CarouselMuseums = _context.Museums.Include(m => m.Region).ToList().OrderBy(m => Guid.NewGuid()).Where(m => m.Picture!= null && m.Picture.Length > 0).ToList().Take(3);
        }
    }
}
