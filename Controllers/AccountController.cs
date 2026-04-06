using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Mom_Project.Models;
using System.Data;

namespace Mom_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Register Page
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string connStr = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // Check existing email
                    using (SqlCommand checkCmd = conn.CreateCommand())
                    {
                        checkCmd.CommandType = CommandType.StoredProcedure;
                        checkCmd.CommandText = "PR_User_SelectByEmail";
                        checkCmd.Parameters.AddWithValue("@Email", model.Email);

                        using (SqlDataReader dr = checkCmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                TempData["ErrorMessage"] = "Email already exists.";
                                return View(model);
                            }
                        }
                    }

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PR_User_Register";

                        cmd.Parameters.AddWithValue("@UserName", model.UserName);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Password", model.Password);
                        cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                        cmd.Parameters.AddWithValue("@Address", model.Address);

                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["SuccessMessage"] = "Registration completed successfully.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error during registration: " + ex.Message;
                return View(model);
            }
        }

        // Login Page
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                string connStr = _configuration.GetConnectionString("ConnectionString");

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "PR_User_Login";

                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@Password", model.Password);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                                HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                                HttpContext.Session.SetString("Email", dr["Email"].ToString());

                                TempData["SuccessMessage"] = "Login successful.";
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                TempData["Error"] = "Invalid email or password.";
                                
                            }
                        }
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error during login: " + ex.Message;
            
                return View(model);
            }
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "Logout successful.";
            return RedirectToAction("Login");


        }
    }
}
