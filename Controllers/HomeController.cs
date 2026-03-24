using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Filters;
using Mom_Project.Models;
using System.Data;
using System.Diagnostics;

namespace Mom_Project.Controllers
{
        #region Index
        [CheckAccess]
        public class HomeController : Controller
        {
            public IActionResult Index()
            {

            int totalMeetings = 0;
            int upcomingMeetings = 0;
            int completedMeetings = 0;
            int cancelledMeetings = 0;

            List<string> typeNames = new List<string>();
            List<int> typeCounts = new List<int>();

            List<string> deptNames = new List<string>();
            List<int> deptCounts = new List<int>();

            string connectionString = "Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("PR_Meetings_DashboardCounts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalMeetings = Convert.ToInt32(reader["TotalMeetings"]);
                            upcomingMeetings = Convert.ToInt32(reader["UpcomingMeetings"]);
                            completedMeetings = Convert.ToInt32(reader["CompletedMeetings"]);
                            cancelledMeetings = Convert.ToInt32(reader["CancelledMeetings"]);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                typeNames.Add(reader["MeetingTypeName"].ToString());
                                typeCounts.Add(Convert.ToInt32(reader["Total"]));
                            }
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                deptNames.Add(reader["DepartmentName"].ToString());
                                deptCounts.Add(Convert.ToInt32(reader["Total"]));
                            }
                        }
                    }
                }
            }

            ViewBag.TotalMeetings = totalMeetings;
            ViewBag.UpcomingMeetings = upcomingMeetings;
            ViewBag.CompletedMeetings = completedMeetings;
            ViewBag.CancelledMeetings = cancelledMeetings;

            ViewBag.TypeNames = typeNames;
            ViewBag.TypeCounts = typeCounts;

            ViewBag.DeptNames = deptNames;
            ViewBag.DeptCounts = deptCounts;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View();
        
            }
        }
        #endregion

}
