using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Dream_Bank.Pages.Clients
{

    public class WithdrawalfunModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration _configuration;

        public WithdrawalfunModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select * from clients where id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.fname = reader.GetString(2);
                                clientInfo.lname = reader.GetString(3);
                                clientInfo.account_no = reader.GetString(4);
                                clientInfo.account_type = reader.GetString(5);
                                clientInfo.email = reader.GetString(13);
                                clientInfo.phone = reader.GetString(14);
                                clientInfo.indeposit = "" + reader.GetInt32(16);

                            }
                        }
                    }
                }
            }
            catch (Exception )
            {

            }
        }


        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.fname = Request.Form["fname"];
            clientInfo.lname = Request.Form["lname"];
            clientInfo.account_no = Request.Form["account_no"];
            clientInfo.account_type = Request.Form["account_type"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.indeposit = Request.Form["indeposit"];



            try
            {

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "update clients " +
                                   "set indeposit= indeposit- @indeposit " +
                                   "where id=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.Parameters.AddWithValue("@fname", clientInfo.fname);
                        command.Parameters.AddWithValue("@lname", clientInfo.lname);
                        command.Parameters.AddWithValue("@account_no", clientInfo.account_no);
                        command.Parameters.AddWithValue("@account_type", clientInfo.account_type);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@indeposit", clientInfo.indeposit);

                        command.ExecuteNonQuery();


                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");
        }
    }
}

