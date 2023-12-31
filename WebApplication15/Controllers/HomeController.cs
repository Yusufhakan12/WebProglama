using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication15.Data;
using WebApplication15.Models;
using WebApplication15.Services;

namespace WebApplication15.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbUcakContext _appDbUcakContext;
        private readonly AppDbBiletContext _appDbBiletContext;
        private  LanguageService _localization;
        public HomeController(AppDbUcakContext appDbUcakContext, AppDbBiletContext appDbBiletContext,LanguageService localization)
        {
            _appDbUcakContext = appDbUcakContext;
            _appDbBiletContext = appDbBiletContext;
            _localization=localization;
        }


        public IActionResult Index()
        {
            ViewBag.Merhaba = _localization.GetKey("Merhaba").Value;
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
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
                var FlysId = seferListesi.FirstOrDefault().VoyageId;
                var FlyCapacity = seferListesi.FirstOrDefault().capacity;
                string prevcapacity = FlyCapacity;
                int newcapacity;
                int.TryParse(prevcapacity, out newcapacity);

                if(newcapacity > 0)
                {
                    HttpContext.Session.SetInt32("voyageid", FlysId);

                    TempData["from"] = model.From;
                    TempData["To"] = model.To;
                    TempData["FromDate"] = model.FromDate.Date;
                    // Sefer listesini view'e gönder
                    return RedirectToAction("Index", "Buy");
                }
                else
                {
                    ViewData["capacityError"] = "Uçak dolu";
                    return View();
                }
               
            }

            ViewData["VoyageError"] = "Sefer bulunamadı";
            return View();
            
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ChangeLangueage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
                ); ;
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}