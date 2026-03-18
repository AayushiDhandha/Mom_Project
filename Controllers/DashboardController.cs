using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Mom_Project.Controllers
{
    
    public class DashboardController : Controller
    {
        #region Dashboard Page
        public IActionResult DashboardPage()
        {
            int meetingTypeCount = 0;
            int deptcount = 0;

            using (SqlConnection conn = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(MeetingTypeId) FROM MOM_MeetingType", conn);
                SqlCommand cmd1 = new SqlCommand("Select COUNT(DepartmentId) From MOM_Department", conn);
                deptcount = (int)cmd.ExecuteScalar();


                meetingTypeCount = (int)cmd.ExecuteScalar();
            }

            ViewBag.MeetingTypeCount = meetingTypeCount;
            ViewBag.DepartmentCount = deptcount;

            return View();
        }
        #endregion
    }
}
