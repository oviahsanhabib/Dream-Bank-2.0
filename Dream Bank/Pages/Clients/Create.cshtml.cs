using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dream_Bank.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


namespace Dream_Bank.Pages.Clients
{

    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public void OnGet()
        {
        }

        public void OnPost(IFormFile img)
        {
            Console.WriteLine("Image file: " + img?.FileName); 
            clientInfo.fname = Request.Form["fname"];
            clientInfo.lname = Request.Form["lname"];
            clientInfo.account_no = Request.Form["account_no"];
            clientInfo.account_type = Request.Form["account_type"];
            clientInfo.gender = Request.Form["gender"];
            clientInfo.dob = Request.Form["dob"];
            clientInfo.address = Request.Form["address"];
            clientInfo.city = Request.Form["city"];
            clientInfo.state = Request.Form["state"];
            clientInfo.postal = Request.Form["postal"];
            clientInfo.country = Request.Form["country"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.ssn = Request.Form["ssn"];
            clientInfo.indeposit = Request.Form["indeposit"];
            clientInfo.img = Request.Form["img"];

            if (img != null && img.Length > 0)
            {

                try

                {
                   
                

                    {
                        

                        byte[] imageBytes;
                        using (MemoryStream ms = new MemoryStream())
                        {

                            img.CopyTo(ms);

                            imageBytes = ms.ToArray();
                        }

                        string connectionString = _configuration.GetConnectionString("DefaultConnection");

                        using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))



                        {
                            using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("INSERT INTO clients (fname, lname,  account_type, gender, dob, address, city, state, postal, country, email, phone, ssn, indeposit, img) values (@fname,@lname, @account_type, @gender, @dob, @address, @city, @state, @postal, @country, @email, @phone, @ssn, @indeposit, @img);", connection))


                            {
                                connection.Open();

                                command.Parameters.AddWithValue("@fname", clientInfo.fname);
                                command.Parameters.AddWithValue("@lname", clientInfo.lname);
                                command.Parameters.AddWithValue("@account_no", clientInfo.account_no);
                                command.Parameters.AddWithValue("@account_type", clientInfo.account_type);
                                command.Parameters.AddWithValue("@gender", clientInfo.gender);
                                command.Parameters.AddWithValue("@dob", clientInfo.dob);
                                command.Parameters.AddWithValue("@address", clientInfo.address);
                                command.Parameters.AddWithValue("@city", clientInfo.city);
                                command.Parameters.AddWithValue("@state", clientInfo.state);
                                command.Parameters.AddWithValue("@postal", clientInfo.postal);
                                command.Parameters.AddWithValue("@country", clientInfo.country);
                                command.Parameters.AddWithValue("@email", clientInfo.email);
                                command.Parameters.AddWithValue("@phone", clientInfo.phone);
                                command.Parameters.AddWithValue("@ssn", clientInfo.ssn);
                                command.Parameters.AddWithValue("@indeposit", clientInfo.indeposit);
                              
                                command.Parameters.Add("@img", SqlDbType.VarBinary, -1).Value = imageBytes;





                                command.ExecuteNonQuery();
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }
            }
            else
            {
                errorMessage = "No image file selected.";
                return;
            }

            clientInfo.fname = ""; clientInfo.lname = ""; clientInfo.account_no = ""; clientInfo.account_type = ""; clientInfo.gender = ""; clientInfo.dob = "";
            clientInfo.address = ""; clientInfo.city = ""; clientInfo.state = ""; clientInfo.postal = ""; clientInfo.country = "";
            clientInfo.email = ""; clientInfo.phone = ""; clientInfo.ssn = ""; clientInfo.indeposit = ""; clientInfo.img = "";
            successMessage = "New Client Added";
            Response.Redirect("/Clients/Index");

        }
    }
}

