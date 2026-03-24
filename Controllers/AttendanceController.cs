using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;

namespace Mom_Project.Controllers
{
    public class AttendanceController : Controller
    {
        #region Attendance List
        public IActionResult AttendanceList()
        {
            List<AttendanceModel> list = new List<AttendanceModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            cmd.CommandText = "PR_Attendance_List";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                AttendanceModel a = new AttendanceModel();

               
                a.MeetingDate = Convert.ToDateTime(reader["MeetingDate"]);
                a.MeetingTypeName = reader["MeetingTypeName"].ToString();
                a.MeetingVenueName = reader["MeetingVenueName"].ToString();
                a.DepartmentName = reader["DepartmentName"].ToString();
                a.StaffName = reader["StaffName"].ToString();
                a.EmailAddress = reader["EmailAddress"].ToString();
                a.AttendanceStatus = reader["Status"].ToString();
                a.Remarks = reader["Remarks"].ToString();
                a.MeetingID = Convert.ToInt32(reader["MeetingID"]);

                list.Add(a);
            }

            con.Close();

            return View(list);
        }
        #endregion

        #region present absent
        public IActionResult MarkPresent(string email, int meetingId)
        {
            using (SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;"))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("PR_Update_Attendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmailAddress", email);
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("AttendanceList");
        }
        #endregion

        #region Export Excel Attendance
        public IActionResult ExportToExcel()
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;"))
                {
                    con.Open();
                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PR_Attendance_List"; 

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                }

                using (var workbook = new ClosedXML.Excel.XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Attendance Report");

                    
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            worksheet.Cell(row + 2, col + 1).Value = dt.Rows[row][col]?.ToString();
                        }
                    }

                    worksheet.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "AttendanceReport.xlsx" 
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting attendance: " + ex.Message;
                return RedirectToAction("AttendanceList");
            }
        }
        #endregion


    }
}
