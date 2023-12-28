using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;

namespace WebApplication15.Controllers
{
    public class BiletlerimController : Controller
    {
        private readonly AppDbBiletContext _appDbBiletContext;
        public BiletlerimController(AppDbBiletContext appDbBiletContext)
        {
            _appDbBiletContext = appDbBiletContext;
        }
        public   async Task<IActionResult> Index()
        {
           

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.userId = HttpContext.Session.GetInt32("UserId");
                int? usId = ViewBag.userId;
                var seferListesi = await _appDbBiletContext.Tickets
                         .Where(u => u.UserId == usId)
                         .ToListAsync();
                return View(seferListesi); 


            }
            return RedirectToAction("Index", "Home");

        }
    }
}
