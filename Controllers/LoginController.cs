using Microsoft.AspNetCore.Mvc;

namespace Mom_Project.Controllers
{
    public class LoginController : Controller
    {
        #region Login Page
        public IActionResult LoginPage()
        {
            return View();
        }
        #endregion

        #region Register Page
        public IActionResult RegisterPage()
        {
            return View();
        }
        #endregion
    }
}
