using Microsoft.AspNetCore.Mvc;
using WebApplication15.Data;
using WebApplication15.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

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
        [HttpPost]

        public async Task<IActionResult> AdminPage(LoginPageViewModel model)
        {
            var email=model.UserName;
            var passwordAdmin=model.PassWord;
     
            var admin = await _appDbContext.Users.FirstOrDefaultAsync(u=>u.UserName==email&u.Password==passwordAdmin&u.Role=="Admin");
            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, email),
                };

                claims.Add(new Claim(ClaimTypes.Role, admin.Role));
                var useridentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("SeferDuzenle", "Ucak"); // Örneğin, başka bir sayfaya yönlendirme
            }
            else
            {
                // Kullanıcı adı bulunamadı, uygun bir işlem gerçekleştir...
                ViewData["Error2"] = "User do not find.";

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult LoginPage()
        {
            return View();
        }
        
        public async Task<IActionResult> LoginPage(LoginPageViewModel model)
        {
            var userName = model.UserName;
            var passWord = model.PassWord;

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName&u.Password==passWord&u.Role=="User");

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId",user.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userName),
                };
                claims.Add(new Claim(ClaimTypes.Role, user.Role));
                var useridentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
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
                    {   Name=addUserRequest.Name,
                        Role=addUserRequest.Role,
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

        [HttpGet]
        public async Task< IActionResult> View(int id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(user!= null)
            {
                var ViewModel = new UpdateUserViewModel()
                {
                    Id = user.Id, // Bu genellikle bir GUID oluşturmak yerine kullanılır. 
                    UserName = user.UserName,
                    UserSurname = user.UserSurname,
                    PassWord = user.Password,
                };

                return await Task.Run(()=> View("View",ViewModel));
            }

            return RedirectToAction("Index", "AdminHomePage");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateUserViewModel model)
        {
            var user = await _appDbContext.Users.FindAsync(model.Id);
            if(user!= null)
            {   
                user.UserName = model.UserName;
                user.UserSurname = model.UserSurname;
                user.Password = model.PassWord;

                await _appDbContext.SaveChangesAsync();

                return RedirectToAction("Index", "AdminHomePage");

            }
            return RedirectToAction("Index", "AdminHomePage");
        }

        public IActionResult Logout()
        {
            // Kullanıcıyı çıkış yapmaya zorla
            HttpContext.SignOutAsync();

            // İsteği başka bir sayfaya yönlendir (isteğe bağlı)
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AdminLogout()
        {
            // Kullanıcıyı çıkış yapmaya zorla
            HttpContext.SignOutAsync();

            // İsteği başka bir sayfaya yönlendir (isteğe bağlı)
            return RedirectToAction("Index", "Home");
        }

    }
}
