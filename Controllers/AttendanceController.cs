using Microsoft.AspNetCore.Mvc;

namespace Mom_Project.Controllers
{
    public class AttendanceController : Controller
    {
        #region Attendance List
        public IActionResult AttendanceList()
        {
            return View();
        }
        #endregion
    }
}
