using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;

namespace WebApplication15.Controllers
{
    public class AdminHomePageController : Controller
    {
        private readonly AppDbContext _appDbContext;
        
        public async Task <IActionResult> Index()
        {
          var users= await _appDbContext.Users.ToListAsync();
            return View(users);
        }
    }
}
