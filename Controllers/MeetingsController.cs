using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;

namespace Mom_Project.Controllers
{
    public class MeetingsController : Controller
    {
        #region Meetings List
        public ActionResult<List<MeetingsModel>> MeetingsList()
        {
            List<MeetingsModel> list = new List<MeetingsModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_Meetings_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MeetingsModel m = new MeetingsModel();
                m.MeetingID = Convert.ToInt32(reader["MeetingID"]);
                m.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);  
                m.MeetingDescription = reader["MeetingDescription"].ToString();
                m.DocumentPath = reader["DocumentPath"].ToString();
                m.IsCancelled = Convert.ToBoolean(reader["IsCancelled"]);
                m.CancellationDateTime =
        reader["CancellationDateTime"] == DBNull.Value
            ? (DateTime?)null
    : Convert.ToDateTime(reader["CancellationDateTime"]);

                m.CancellationReason = reader["CancellationReason"].ToString();


                list.Add(m);
            }

            reader.Close();
            con.Close();
            return View(list);
        }
        #endregion

        #region Meetings Add Edit
        public IActionResult MeetingsAddEdit()
        {
            ViewBag.MeetingTypeList = FillMeetingTypeDropDown();
            ViewBag.DepartmentList = FillDepartmentDropDown();
            ViewBag.MeetingVenueList = FillMeetingVenueDropDown();
            return View();
        }
        #endregion

        #region Meetings Delete
        public IActionResult DeleteMeetings(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "PR_Meetings_DeleteByPk";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = new SqlParameter();
                p.ParameterName = "@MeetingID";
                p.SqlDbType = SqlDbType.Int;
                p.Value = id;

                cmd.Parameters.Add(p);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["Success"] = "Delete Successfully.";
                
                return RedirectToAction("MeetingsList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Foreign Key Constraint Violated.";
                return RedirectToAction("MeetingsList");
            }
        }
        #endregion

        #region Save
        public IActionResult Save(MeetingsModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("MeetingsAddEdit", model);
            }
            return RedirectToAction("MeetingsList", model);
        }
        #endregion

        #region Drop Down
        public List<SelectListItem> FillMeetingTypeDropDown()
        {

            List<SelectListItem> meetingTypeList = new List<SelectListItem>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_MeetingType_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                meetingTypeList.Add(new SelectListItem(reader["MeetingTypeName"].ToString(), reader["MeetingTypeID"].ToString()));
            }

            reader.Close();
            con.Close();

            return meetingTypeList;
        }

        public List<SelectListItem> FillDepartmentDropDown()
        {

            List<SelectListItem> deptList = new List<SelectListItem>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_Department_DropDown";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                deptList.Add(new SelectListItem(reader["DepartmentName"].ToString(), reader["DepartmentID"].ToString()));
            }

            reader.Close();
            con.Close();

            return deptList;
        }

        public List<SelectListItem> FillMeetingVenueDropDown()
        {

            List<SelectListItem> venueList = new List<SelectListItem>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_MeetingVenue_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                venueList.Add(new SelectListItem(reader["MeetingVenueName"].ToString(), reader["MeetingVenueID"].ToString()));
            }

            reader.Close();
            con.Close();

            return venueList;
        }
        #endregion
       
    }
}
