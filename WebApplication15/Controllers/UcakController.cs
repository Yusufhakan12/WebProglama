using Microsoft.AspNetCore.Mvc;
using WebApplication15.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication15.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication15.Controllers
{

  
    public class UcakController : Controller
    {
        private readonly AppDbUcakContext _appDbUcakContext;
        public UcakController(AppDbUcakContext appDbUcakContext)
        {
            _appDbUcakContext = appDbUcakContext;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UcakEkle()
        {      
            if(User.IsInRole("Admin"))
            {
                return View();

            }
            else
            {
                return RedirectToAction("SeferDuzenle", "Ucak");
            }


        }
        [HttpPost]
        public async Task<IActionResult> UcakEkle(SeferEkleViewModel model)
        {
            var Flys = await _appDbUcakContext.Voyages.FirstOrDefaultAsync(x => x.AirPlaneName == model.AirPlaneName && model.From == x.From && model.To == x.To&& model.FromDate.Day - x.FromDate.Day > 3);
            if(Flys != null)
            {
                return RedirectToAction("SeferDuzenle", "Ucak");
            }
            var donus=await _appDbUcakContext.Voyages.FirstOrDefaultAsync(x => (x.AirPlaneName == model.AirPlaneName && model.To==x.From&&model.From==x.To)&&(model.FromDate.Day-x.FromDate.Day>0));
            if(donus!=null)
            {   
                var Voyage = new Voyage
                {
                    VoyageId = model.VoyageId.GetHashCode(),

                    From = model.From,
                    To = model.To,
                    AirPlaneName = model.AirPlaneName,
                    FromDate = model.FromDate.ToUniversalTime(),
                    capacity = model.capacity,




                };
                await _appDbUcakContext.Voyages.AddAsync(Voyage);
                await _appDbUcakContext.SaveChangesAsync();

                return RedirectToAction("SeferDuzenle", "Ucak");
            }
            if(donus==null&&Flys==null)
            {
                var Voyage = new Voyage
                {
                    VoyageId = model.VoyageId.GetHashCode(),

                    From = model.From,
                    To = model.To,
                    AirPlaneName = model.AirPlaneName,
                    FromDate = model.FromDate.ToUniversalTime(),
                    capacity = model.capacity,




                };
                await _appDbUcakContext.Voyages.AddAsync(Voyage);
                await _appDbUcakContext.SaveChangesAsync();
               
                return RedirectToAction("SeferDuzenle", "Ucak");
            }

            return View("Böyle bir sefer var");
          
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeferDuzenle(SeferEkleViewModel model)
        {   

            var Flys = await _appDbUcakContext.Voyages.ToListAsync();
            return View(Flys);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeferGuncelle(int id)
        {
            var Flys = await _appDbUcakContext.Voyages.FirstOrDefaultAsync(x => x.VoyageId == id);

            if (Flys != null)
            {
                var ViewModel = new SeferUpdateViewModel()
                {
                  VoyageId=Flys.VoyageId,
                  From = Flys.From,
                  FromDate = Flys.FromDate.ToUniversalTime(),
                  To = Flys.To,
                  AirPlaneName = Flys.AirPlaneName,
                  capacity=Flys.capacity
                };

                return await Task.Run(() => View("SeferGuncelle", ViewModel));
            }

            return RedirectToAction("SeferDuzenle", "Ucak");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SeferGuncelle(SeferUpdateViewModel model)
        {
            var Flys = await _appDbUcakContext.Voyages.FindAsync(model.VoyageId);
            if (Flys != null)
            {
                Flys.capacity = model.capacity;
                Flys.FromDate=model.FromDate.ToUniversalTime();
                Flys.To = model.To;
                Flys.AirPlaneName = Flys.AirPlaneName;
                Flys.From=model.From;


                await _appDbUcakContext.SaveChangesAsync();

                return RedirectToAction("SeferDuzenle", "Ucak");

            }
            return RedirectToAction("SeferDuzenle", "Ucak");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteVoyage(SeferUpdateViewModel model)
        {
            var Flys = await _appDbUcakContext.Voyages.FindAsync(model.VoyageId);
            if(Flys!=null)
            {
                _appDbUcakContext.Voyages.Remove(Flys);
                await _appDbUcakContext.SaveChangesAsync();
                return RedirectToAction("SeferDuzenle", "Ucak");
            }
            return RedirectToAction("SeferDuzenle", "Ucak");
        }
    }
}
