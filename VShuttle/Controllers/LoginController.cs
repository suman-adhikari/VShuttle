using System.Web.Mvc;
using VShuttle.Models;
using VShuttle.Repository;

namespace VShuttle.Controllers
{
    public class LoginController : Controller
    {
        UserRepository loginRepository = new UserRepository();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public LoginController() {
            
        }

        [HttpPost]
        public ActionResult Index(Users users) {

            if (users.UserId != null)
            {
                var loginData = loginRepository.RegisterUser(users);
                if(loginData)
                    return RedirectToAction("Index", "Admin");
                else
                {
                    ViewData["error"] = "Registration failed !!!";
                    return RedirectToAction("Register", "Login");
                }
            }
            else
            {
                var loginData = loginRepository.CheckUser(users.UserName, users.Password);
                if (loginData != null)
                {
                    Session["Id"] = loginData.Id;
                    Session["UserId"] = loginData.UserId;
                    Session["UserName"] = loginData.UserName;
                    Session["UserRole"] = loginData.UserRole;
                    if(loginData.UserRole==1)
                      return RedirectToAction("Index", "Admin");
                    return RedirectToAction("Index", "Home");
                }
                ViewData["error"] = "Invalid username or password !!!";
            }
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.register = 1;
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
       
    }
}