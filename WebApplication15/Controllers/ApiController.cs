using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication15.Data;
using WebApplication15.Models;

namespace WebApplication15.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly AppDbUcakContext _appDbUcakContext;
        public ApiController(AppDbUcakContext appDbUcakContext)
        {
            _appDbUcakContext = appDbUcakContext;
        }

         [HttpGet]
        public List<Voyage> Get()
        {
            return _appDbUcakContext.Voyages.ToList();
        }
        [HttpGet("{id}")]
        public List<Voyage> Get(int id)
        {
            return _appDbUcakContext.Voyages.ToList();
        }
        [HttpPost]
        public void Post([FromBody] SeferEkleViewModel v)
        {
            _appDbUcakContext.Add(v);
            _appDbUcakContext.SaveChanges();

        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var voyage= _appDbUcakContext.Voyages.FirstOrDefault(x=>x.VoyageId==id);

            _appDbUcakContext.Voyages.Remove(voyage);
            _appDbUcakContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}
