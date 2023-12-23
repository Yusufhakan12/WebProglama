using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    public class AdminHomePageController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public AdminHomePageController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
                var users = await _appDbContext.Users.ToListAsync();
                return View(users);
            
           
          
        }
    }
}
