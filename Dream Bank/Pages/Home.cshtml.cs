using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace Dream_Bank.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public HomeModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int ClientsNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalTransactionCount { get; set; } // Add property to store total transaction count

        public void OnGet()
        {
          
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to get total number of clients
                    string countQuery = "SELECT COUNT(*) FROM Clients";

                    using (SqlCommand countCommand = new SqlCommand(countQuery, connection))
                    {
                        // ExecuteScalar retrieves the first column of the first row of the result set
                        // This will be the total number of clients
                        ClientsNumber = (int)countCommand.ExecuteScalar();
                    }

                    // SQL query to get total amount from Transactions table
                    string totalAmountQuery = "SELECT SUM(amount) FROM Transactions";

                    using (SqlCommand totalAmountCommand = new SqlCommand(totalAmountQuery, connection))
                    {
                        // ExecuteScalar retrieves the first column of the first row of the result set
                        // This will be the total amount
                        object result = totalAmountCommand.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            TotalAmount = Convert.ToDecimal(result);
                        }
                        else
                        {
                            TotalAmount = 0; // Set total amount to 0 if there are no transactions
                        }
                    }

                    // SQL query to get total transaction count for today
                    string totalTransactionCountQuery = "SELECT COUNT(*) FROM Transactions WHERE CONVERT(date, [date]) = CONVERT(date, GETDATE())";

                    using (SqlCommand totalTransactionCountCommand = new SqlCommand(totalTransactionCountQuery, connection))
                    {
                        // ExecuteScalar retrieves the first column of the first row of the result set
                        // This will be the total transaction count for today
                        TotalTransactionCount = (int)totalTransactionCountCommand.ExecuteScalar();
                    }

                    // Rest of your code...
                }
            }
            catch (Exception)
            {
                // Handle exceptions
            }
        }
    }
}
