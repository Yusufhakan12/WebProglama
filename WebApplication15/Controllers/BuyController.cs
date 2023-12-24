using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    public class BuyController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly AppDbUcakContext _appDbUcakContext;
        private readonly AppDbBiletContext _appDbBiletContext;
        public BuyController(AppDbContext appDbContext, AppDbUcakContext appDbUcakContext, AppDbBiletContext appDbBiletContext)
        {
            _appDbContext = appDbContext;
            _appDbUcakContext = appDbUcakContext;
            _appDbBiletContext = appDbBiletContext;
        }
        public IActionResult Index()
        {   
           
           return View();
  
        }
        [HttpPost]
        public async Task<IActionResult>Index(BuyViewModel buyView)
        {
            ViewBag.voyageid = HttpContext.Session.GetInt32("voyageid");
            int?voy=ViewBag.voyageid;
            
            var isHave = await _appDbBiletContext.Tickets.FirstOrDefaultAsync(x => x.SeatId == buyView.SeatId && x.VoyageId ==voy);

            
            if (isHave!=null)
            {
                ViewData["Error3"] = "Koltuk dolu";
                return View();
            }
            else
            {
                ViewBag.userId = HttpContext.Session.GetInt32("UserId");
                
                var ticket = new Ticket
                {
                    Id = buyView.GetHashCode(),
                    VoyageId = ViewBag.voyageid,
                    UserId = ViewBag.userId,
                    UserName = buyView.UserName,
                    UserSurName = buyView.UserSurName,
                    SeatId = buyView.SeatId,

                };
                await _appDbBiletContext.Tickets.AddAsync(ticket);
                await _appDbBiletContext.SaveChangesAsync();

                // Başka bir işlem yapılabilir, örneğin kullanıcıyı başka bir sayfaya yönlendirebilirsiniz
                return RedirectToAction("Index", "Home");
            }
           
        }

       

    }
}
