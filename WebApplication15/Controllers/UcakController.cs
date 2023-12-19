using Microsoft.AspNetCore.Mvc;
using WebApplication15.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;

namespace WebApplication15.Controllers
{

  
    public class UcakController : Controller
    {
        private readonly AppDbUcakContext _appDbUcakContext;

        public UcakController(AppDbUcakContext appDbUcakContext)
        {
            _appDbUcakContext = appDbUcakContext;
        }
        [HttpGet]

        public async Task<IActionResult> UcakEkle()
        {
            var Flys = await _appDbUcakContext.FlyNames.ToListAsync();
            return View(Flys);
        }
        [HttpPost]
        public async Task<IActionResult> UcakEkle(UcakEkleViewModel model)
        {
            var FlyName = new FlyName
            {
                Id = model.Id.GetHashCode(),
                Name = model.Name,

            };
            await _appDbUcakContext.FlyNames.AddAsync(FlyName);
            await _appDbUcakContext.SaveChangesAsync();

            return RedirectToAction("UcakEkle", "Ucak");
        }
    }
}
