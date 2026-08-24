using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Dream_Bank.Pages.Clients
{
    public class NewAdminModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public NewAdminModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
           
        }

        public void OnPost(string name, string email, string password)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Insert the new admin into the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (name, email, password) VALUES (@Name, @Email, @Password)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.ExecuteNonQuery();
                }
            }

            Message = "New admin added successfully!";
            Response.Redirect("/Clients/NewAdmin");
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}