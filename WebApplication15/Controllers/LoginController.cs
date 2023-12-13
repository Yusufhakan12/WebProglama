using Microsoft.AspNetCore.Mvc;
using WebApplication15.Data;
using WebApplication15.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication15.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public LoginController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public IActionResult AdminPage()
        {
            return View();
        }


        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginPage(LoginPageViewModel model)
        {
            var userName = model.UserName;
            var passWord = model.PassWord;

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName&u.Password==passWord);

            if (user != null)
            {
                // Kullanıcı adı bulundu, işlemlerinizi gerçekleştirin...

                return RedirectToAction("Index", "Home"); // Örneğin, başka bir sayfaya yönlendirme
            }
            else
            {
                // Kullanıcı adı bulunamadı, uygun bir işlem gerçekleştir...
                ViewData["Error2"] = "User do not find.";

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task< IActionResult> Add(AddUserViewModel addUserRequest)
        {
            if (ModelState.IsValid)
            {
                if (addUserRequest.ConfirmPassWord != addUserRequest.PassWord)
                {
                    ViewData["Error"] = "Passwords do not match.";
                }
                else
                {


                    var user = new User()
                    {
                        Id = addUserRequest.GetHashCode(), // Bu genellikle bir GUID oluşturmak yerine kullanılır. 
                        UserName = addUserRequest.UserName,
                        UserSurname=addUserRequest.UserSurname,
                        Password = addUserRequest.PassWord
                    };

                    // _appDbContext üzerinden veritabanına kullanıcıyı ekleyin
                    await _appDbContext.Users.AddAsync(user);
                    await _appDbContext.SaveChangesAsync();

                    // Başka bir işlem yapılabilir, örneğin kullanıcıyı başka bir sayfaya yönlendirebilirsiniz
                    return RedirectToAction("LoginPage", "Login");
                }
            }

            // ModelState.IsValid false ise, kullanıcıya hata mesajları ile Add sayfasını tekrar göster
            return View(addUserRequest);
        }
    }
}
