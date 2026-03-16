using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;

namespace Mom_Project.Controllers
{
    public class MeetingVenueController : Controller
    {
        #region Meeting Venue List
        public ActionResult<List<MeetingVenueModel>> MeetingVenueList()
        {
            List<MeetingVenueModel> list = new List<MeetingVenueModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_MeetingVenue_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MeetingVenueModel m = new MeetingVenueModel();
                m.MeetingVenueID = Convert.ToInt32(reader["MeetingVenueID"]);
                m.MeetingVenueName = reader["MeetingVenueName"].ToString();


                list.Add(m);
            }

            reader.Close();
            con.Close();
            return View(list);
        }
        #endregion

        #region Meeting Venue Add Edit
        [HttpGet]
        public IActionResult MeetingVenueAddEdit(int? id)
        {
            if (id > 0)
            {
                MeetingVenueModel meetingVenue = GetDepartmentById(id.Value);
                return View(meetingVenue);
            }
            else
            {
                return View(new MeetingVenueModel());
            }
        }
        #endregion

        #region Get Venue By Id
        public MeetingVenueModel GetDepartmentById(int id)
        {
            MeetingVenueModel meetingVenue = new MeetingVenueModel();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand("PR_MeetingVenue_SelectByPk", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MeetingVenueID", id);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                meetingVenue.MeetingVenueID = Convert.ToInt32(reader["MeetingVenueID"]);
                meetingVenue.MeetingVenueName = reader["MeetingVenueName"].ToString();
            }

            reader.Close();
            con.Close();

            return meetingVenue;
        }
        #endregion

        #region Meeting Venue Delete
        public IActionResult DeleteMeetingVenue(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "PR_MeetingVenue_DeleteByPk";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = new SqlParameter();
                p.ParameterName = "@MeetingVenueID";
                p.SqlDbType = SqlDbType.Int;
                p.Value = id;

                cmd.Parameters.Add(p);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["Success"] = "Delete Successfully.";

                return RedirectToAction("MeetingVenueList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Foreign Key Constraint Violated.";
                return RedirectToAction("MeetingVenueList");
            }
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(MeetingVenueModel model)
        { 
            try {
                if (!ModelState.IsValid) { 
                    return View("MeetingVenueAddEdit", model); 
            } 
                 
                 SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;"); 
                SqlCommand cmd = new SqlCommand(); 
                cmd.Connection = con; 

                if (model.MeetingVenueID == 0) 
                { 
                    cmd.CommandText = "PR_MeetingVenue_Insert";
                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                    TempData["Success"] = "Meeting Venue added successfully"; 
                } 
                else 
                { 
                    cmd.CommandText = "PR_MeetingVenue_UpdateByPk"; 
                    cmd.Parameters.AddWithValue("@MeetingVenueID", model.MeetingVenueID); 
                    TempData["Success"] = "Meeting Venue updated successfully"; 
                } 
              
                cmd.CommandType = CommandType.StoredProcedure;
                
                SqlParameter mv = new SqlParameter();
                mv.ParameterName = "@MeetingVenueName";
                mv.SqlDbType = SqlDbType.NVarChar;
                mv.Value = model.MeetingVenueName;

                cmd.Parameters.Add(mv); 
                
                cmd.Parameters.AddWithValue("@Modified", DateTime.Now);
                
                con.Open(); 
                int noOfRows = cmd.ExecuteNonQuery();
                con.Close(); 
                return RedirectToAction("MeetingVenueList");
            }
            catch (Exception ex) {
                TempData["Error"] = ex.Message; 
                return RedirectToAction("MeetingVenueList"); 
            }
        }
        #endregion

        #region Search

        [HttpPost]
        public IActionResult MeetingVenueList(IFormCollection formData)
        {
            string searchText = formData["SearchText"].ToString();

            if (string.IsNullOrWhiteSpace(searchText))
                searchText = null;

            ViewBag.SearchText = searchText;

            List<MeetingVenueModel> list = GetMeetingVenue(searchText);
            return View(list);
        }

        private List<MeetingVenueModel> GetMeetingVenue(string searchText)
        {
            List<MeetingVenueModel> list = new List<MeetingVenueModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_MeetingVenue_SelectAll";
            cmd.CommandType = CommandType.StoredProcedure;

            if (searchText != null)
                cmd.Parameters.AddWithValue("@SearchText", searchText);
            else
                cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MeetingVenueModel m = new MeetingVenueModel();
                m.MeetingVenueID = Convert.ToInt32(reader["MeetingVenueID"]);
                m.MeetingVenueName = reader["MeetingVenueName"].ToString();

                list.Add(m);
            }

            reader.Close();
            con.Close();

            return list;
        }

        #endregion

        #region Export Excel

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
                        cmd.CommandText = "PR_MeetingVenue_SelectAll";
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("MeetingVenue");

                    // Header row
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = dt.Columns[i].ColumnName;
                        worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    }

                    // Data rows
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
                            "MeetingVenueList.xlsx"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("MeetingVenueList");
            }

        }
        #endregion
    }
}
