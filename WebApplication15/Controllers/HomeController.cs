using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication15.Data;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbUcakContext _appDbUcakContext;
        private readonly AppDbBiletContext _appDbBiletContext;

        public HomeController(AppDbUcakContext appDbUcakContext, AppDbBiletContext appDbBiletContext)
        {
            _appDbUcakContext = appDbUcakContext;
            _appDbBiletContext = appDbBiletContext;
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BiletIslemleri()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();


            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> BiletIslemleri(SeferEkleViewModel model)
        {   
            var seferListesi = await _appDbUcakContext.Voyages
                    .Where(u => u.From == model.From && u.To == model.To && u.FromDate.ToUniversalTime() == model.FromDate.ToUniversalTime())
                    .ToListAsync();
            
            // Formdan gelen verileri kontrol et
            if (seferListesi.Any())
            {
                var yeterid = seferListesi.FirstOrDefault().VoyageId;

                // Veritabanında sorgu yap
                HttpContext.Session.SetInt32("voyageid",yeterid);
                TempData["from"] = model.From;
                TempData["To"] = model.To;
                TempData["FromDate"] = model.FromDate.Date;
                // Sefer listesini view'e gönder
                return RedirectToAction("Index", "Buy");
            }

            // Form geçersizse aynı sayfaya geri dön
            return RedirectToAction("BiletIslemleri", "Home");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}