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
        [HttpPost]
        public async Task<IActionResult> AdminPage(AdminPageViewModel model)
        {
            var email=model.Email;
            var passwordAdmin=model.PassWord;
            var user = await _appDbContext.Admins.FirstOrDefaultAsync(u=>u.Email==email&u.Password==passwordAdmin);
            if (user != null)
            {
                // Kullanıcı adı bulundu, işlemlerinizi gerçekleştirin...

                return RedirectToAction("Index", "AdminHomePage"); // Örneğin, başka bir sayfaya yönlendirme
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
    }
}
