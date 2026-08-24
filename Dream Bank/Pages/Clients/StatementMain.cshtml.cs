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

    public class StatementMainModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        private readonly IConfiguration _configuration;

        public StatementMainModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "Select * from clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.fname = reader.GetString(2);
                                clientInfo.lname = reader.GetString(3);
                                clientInfo.account_no = reader.GetString(4);
                                clientInfo.account_type = reader.GetString(5);
                                clientInfo.gender = reader.GetString(6);
                                clientInfo.dob = reader.GetString(7);
                                clientInfo.address = reader.GetString(8);
                                clientInfo.city = reader.GetString(9);
                                clientInfo.state = reader.GetString(10);
                                clientInfo.postal = reader.GetString(11);
                                clientInfo.country = reader.GetString(12);
                                clientInfo.email = reader.GetString(13);
                                clientInfo.phone = reader.GetString(14);
                                clientInfo.ssn = reader.GetString(15);
                                clientInfo.indeposit = "" + reader.GetInt32(16);
                                clientInfo.created_at = reader.GetDateTime(17).ToString();

                                listClients.Add(clientInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }


        }
    }

    public class ClientInfo4
    {
        public String id;
        public String fname;
        public String lname;
        public String account_no;
        public String account_type;
        public String gender;
        public String dob;
        public String address;
        public String city;
        public String state;
        public String postal;
        public String country;
        public String email;
        public String phone;
        public String ssn;
        public String indeposit;
        public String created_at;
    }
}
