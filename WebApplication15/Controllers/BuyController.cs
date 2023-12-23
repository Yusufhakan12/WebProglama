using Microsoft.AspNetCore.Mvc;

namespace WebApplication15.Controllers
{
    public class BuyController : Controller
    {
        
        public IActionResult Index()
        {   
            if(User.Identity.IsAuthenticated)
            return View();

            return RedirectToAction("Home","Index");
        }
    }
}
