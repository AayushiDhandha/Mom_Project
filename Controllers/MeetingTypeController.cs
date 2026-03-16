using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;

namespace Mom_Project.Controllers
{
    public class MeetingTypeController : Controller
    {
        #region Meeting Type List
        public ActionResult<List<MeetingTypeModel>> MeetingTypeList()
        {
            List<MeetingTypeModel> list = new List<MeetingTypeModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_Type";
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MeetingTypeModel m = new MeetingTypeModel();
                m.MeetingTypeID = Convert.ToInt32(reader["MeetingTypeID"]);
                m.MeetingTypeName = reader["MeetingTypeName"].ToString();
                m.Remarks = reader["Remarks"].ToString();


                list.Add(m);
            }

            reader.Close();
            con.Close();
            return View(list);
        }
        #endregion

        #region Meeting Type Add Edit
        [HttpGet]
        public IActionResult MeetingTypeAddEdit(int? id)
        {
            if (id > 0)
            {
                MeetingTypeModel meetingType = GetMeetingTypeById(id.Value);
                return View(meetingType);
            }
            else
            {
                return View(new MeetingTypeModel());
            }
        }
        #endregion

        #region Get Meeting Type By Id
        public MeetingTypeModel GetMeetingTypeById(int id)
        {
            MeetingTypeModel meetingType = new MeetingTypeModel();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand("PR_SbyPk", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@MeetingTypeID", id);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                meetingType.MeetingTypeID = Convert.ToInt32(reader["MeetingTypeID"]);
                meetingType.MeetingTypeName = reader["MeetingTypeName"].ToString();
                meetingType.Remarks = reader["Remarks"].ToString();
            }

            reader.Close();
            con.Close();

            return meetingType;
        }
        #endregion

        #region Meeting Type Delete
        public IActionResult DeleteMeetingType(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "PR_DbyPk";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter p = new SqlParameter();
                p.ParameterName = "@MeetingTypeID";
                p.SqlDbType = SqlDbType.Int;
                p.Value = id;

                cmd.Parameters.Add(p);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["Success"] = "Delete Successfully.";

                return RedirectToAction("MeetingTypeList");
            }
            catch (Exception)
            {
                TempData["Error"] = "Foreign Key Constraint Violated.";
                return RedirectToAction("MeetingTypeList");
            }
        }
        #endregion

        #region Save
        [HttpPost]
        public IActionResult Save(MeetingTypeModel model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("MeetingTypeAddEdit", model);
                }

                SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");
                SqlCommand cmd = new SqlCommand(); 
                cmd.Connection = con;

                if (model.MeetingTypeID == 0)
                {
                    cmd.CommandText = "PR_In";
                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                    TempData["Success"] = "Meeting Type added successfully";
                }
                else
                {
                    cmd.CommandText = "PR_UpbyPk";
                    cmd.Parameters.AddWithValue("@MeetingTypeID", model.MeetingTypeID);
                    
                    TempData["Success"] = "Meeting Type updated successfully";
                }

                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter meetingTypeName = new SqlParameter();
                meetingTypeName.ParameterName = "@MeetingTypeName";
                meetingTypeName.SqlDbType = SqlDbType.NVarChar;
                meetingTypeName.Value = model.MeetingTypeName;

                cmd.Parameters.Add(meetingTypeName);

                SqlParameter remarks = new SqlParameter();
                remarks.ParameterName = "@Remarks";
                remarks.SqlDbType = SqlDbType.NVarChar;
                remarks.Value = model.Remarks;

                cmd.Parameters.Add(remarks);

                cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

                con.Open();
                int noOfRows = cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("MeetingTypeList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("MeetingTypeList");
            }
        }
        #endregion

        #region Search

        [HttpPost]
        public IActionResult MeetingTypeList(IFormCollection formData)
        {
            string searchText = formData["SearchText"].ToString();

            if (string.IsNullOrWhiteSpace(searchText))
                searchText = null;

            ViewBag.SearchText = searchText;

            List<MeetingTypeModel> list = GetMeetingType(searchText);
            return View(list);
        }

        private List<MeetingTypeModel> GetMeetingType(string searchText)
        {
            List<MeetingTypeModel> list = new List<MeetingTypeModel>();

            SqlConnection con = new SqlConnection("Server=AAYUSHI-DHANDHA\\SQLEXPRESS;Database=DOTNET_PROJECT;Trusted_Connection=True;TrustServerCertificate=True;");

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "PR_Type";
            cmd.CommandType = CommandType.StoredProcedure;

            if (searchText != null)
                cmd.Parameters.AddWithValue("@SearchText", searchText);
            else
                cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                MeetingTypeModel m = new MeetingTypeModel();
                m.MeetingTypeID = Convert.ToInt32(reader["MeetingTypeID"]);
                m.MeetingTypeName = reader["MeetingTypeName"].ToString();
                m.Remarks = reader["Remarks"].ToString();

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
                        cmd.CommandText = "PR_Type";
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dt.Load(dr);
                        }
                    }
                }

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("MeetingType");

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
                            "MeetingTypeList.xlsx"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("MeetingTypeList");
            }

        }
        #endregion
    }

}