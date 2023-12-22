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

        public HomeController(AppDbUcakContext appDbUcakContext)
        {
            _appDbUcakContext = appDbUcakContext;
        }
       

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BiletIslemleri()
        {
            return View();
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
                // Veritabanında sorgu yap
                

                // Sefer listesini view'e gönder
                return RedirectToAction("Index", "Home");
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