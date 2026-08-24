using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace Dream_Bank.Pages.Clients
{

    public class StatementModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public StatementModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Transaction> Transactions { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }
        public DateTime MaxDate { get; private set; }
        public DateTime MinDate { get; private set; }
        public void OnGet()
        {
            string userID = Request.Query["userID"];


            Transactions = new List<Transaction>();

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT t.*, c.fname, c.lname FROM Transactions t " +
                                 "JOIN Clients c ON t.UserId = c.id " +
                                 "WHERE t.userID = @userID " +
                                 "ORDER BY t.Date DESC";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
          

                        Console.WriteLine($"SQL Query: {sql}");


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transactions.Add(new Transaction
                                {
                                    TransactionId = reader.GetInt32(0),
                                    UserId = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    Amount = reader.GetDecimal(3),
                                    TransactionType = reader.GetString(4),
                                    Date = reader.GetDateTime(5),
                                    ClientFirstName = reader.GetString(6),
                                    ClientLastName = reader.GetString(7)
                                });
                            }
                        }
                    }

                    string maxDateSql = "SELECT CONVERT(date, MAX(Date)) FROM Transactions WHERE UserID = @userID";
                    using (SqlCommand maxDateCommand = new SqlCommand(maxDateSql, connection))
                    {
                        maxDateCommand.Parameters.AddWithValue("@userID", userID);
                        object maxDateResult = maxDateCommand.ExecuteScalar();
                        if (maxDateResult != null && maxDateResult != DBNull.Value)
                        {
                            MaxDate = (DateTime)maxDateResult;
                        }
                    }

                    string minDateSql = "SELECT CONVERT(date, MIN(Date)) FROM Transactions WHERE UserID = @userID";
                    using (SqlCommand minDateCommand = new SqlCommand(minDateSql, connection))
                    {
                        minDateCommand.Parameters.AddWithValue("@userID", userID);
                        object minDateResult = minDateCommand.ExecuteScalar();
                        if (minDateResult != null && minDateResult != DBNull.Value)
                        {
                            MinDate = (DateTime)minDateResult;
                        }
                    }

                }

            }
            catch (Exception ex)
            
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }


        }
        public class Transaction
        {
            public int TransactionId { get; set; }
            public string UserId { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public string TransactionType { get; set; }
            public DateTime Date { get; set; }
            public string ClientFirstName { get; set; }
            public string ClientLastName { get; set; }
        }

        public decimal TotalBalance
        {
            get
            {
                // Calculate total balance by summing up transaction amounts
                decimal total = 0;
                foreach (var transaction in Transactions)
                {
                    if (transaction.TransactionType == "Debit")
                    {
                        total += transaction.Amount;
                    }
                    else if (transaction.TransactionType == "Credit")
                    {
                        total += transaction.Amount;
                    }
                }
                return total;

            }
        }

    }
    
}