using Microsoft.AspNetCore.Mvc;

namespace Mom_Project.Controllers
{
    public class DashboardController : Controller
    {
        #region Dashboard Page
        public IActionResult DashboardPage()
        {
            return View();
        }
        #endregion
    }
}
