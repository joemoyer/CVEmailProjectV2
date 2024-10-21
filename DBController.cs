using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace EmailAPI
{
    public class DBController
    {
        SqlConnection connection = new SqlConnection();
        string hostemail = "";
        public DBController(IConfiguration configuration){
            this.hostemail = configuration["HostEmail"];
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            this.connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public void addEmailAttempt(string email, string subject, string body){
            string timeStamp = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            string query = "insert into emaillog(hostemail,recipientemail, emailsubject, emailbody,timesent) values (@hostemail,@recipientemail, @emailsubject,@emailbody,@timestamp)";
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@hostemail", hostemail);
                command.Parameters.AddWithValue("@recipientemail", email);
                command.Parameters.AddWithValue("@emailsubject", subject);
                command.Parameters.AddWithValue("@emailbody", body);
                command.Parameters.AddWithValue("@timestamp", timeStamp);

                int result = command.ExecuteNonQuery();

                if(result < 0)
                    Console.WriteLine("Error inserting data into Database!");
            }
        }
    }
}