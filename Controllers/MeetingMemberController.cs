using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;

namespace Mom_Project.Controllers
{
    public class MeetingMemberController : Controller
    {
        #region Meeting Member List 
        public ActionResult<List<MeetingMemberModel>>MeetingMemberList()
        {
            List<MeetingMemberModel> meetingMemberlist = new List<MeetingMemberModel>();
            //List<StaffModel> staffList = new List<StaffModel>();
            //List<MeetingsModel> meetingsList = new List<MeetingsModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_MeetingMember_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) { 
                MeetingMemberModel m = new MeetingMemberModel();
                //StaffModel s = new StaffModel();
                //MeetingsModel meetings = new MeetingsModel();

                m.MeetingMemberID = Convert.ToInt32(reader["MeetingMemberID"]);
                //meetings.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                //s.StaffName = reader["StaffName"].ToString();
                m.IsPresent = Convert.ToBoolean(reader["IsPresent"]);
                m.Remarks = reader["Remarks"].ToString();
             

                meetingMemberlist.Add(m);
                //staffList.Add(s);
                //meetingsList.Add(meetings);
            }

            reader.Close();
            con.Close();

            return View(meetingMemberlist);
        }
        #endregion

        #region Meeting Member Add Edit
        public IActionResult MeetingMemberAddEdit()
        {
            return View();
        }
        #endregion

        #region Save
        public IActionResult Save(MeetingMemberModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("MeetingMemberAddEdit", model);
            }
            return RedirectToAction("MeetingMemberList", model);
        }
        #endregion

        #region Meeting Member Delete
        public IActionResult DeleteMeetingMember(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "PR_MeetingMember_DeleteByPk";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = new SqlParameter();
                p.ParameterName = "@MeetingMemberID";
                p.SqlDbType = SqlDbType.Int;
                p.Value = id;

                cmd.Parameters.Add(p);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["Success"] = "Delete Successfully.";
                return RedirectToAction("MeetingMemberList");
            }
            catch(Exception)
            {
                TempData["Error"] = "Foreign Key Constraint Violated.";
                return RedirectToAction("Index");
            }
        }
        #endregion

    }
}
