using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Dapper;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    //[Authorize(Roles = "Administrator")]
    
    [Route("api/[controller]")]
    [ApiController]
    public class FirstAppController : ControllerBase
    {
        private IConfiguration i_config;
        public FirstAppController(IConfiguration c)
        {
            i_config = c;
        }
        [HttpGet]
        [Route("GetInfo")]
        public JsonResult GetInfo()
        {
            string query = "select * from dbo.Users";
            DataTable table=new DataTable();
            string sqlDatasource = i_config.GetConnectionString("DefaultConnection");
            SqlDataReader reader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    reader= cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    mycon.Close();

                }
            }
            return new JsonResult(table);

        }
       
        [HttpPost]
        [Route("AddUser")]
        public ActionResult AddUser([FromBody]User user)
        {
            string query = "INSERT INTO dbo.Users (fullname, email, password, roleId) VALUES (@fullname, @email, @password, @roleId)";
           

            // Қосылым жолы
            string sqlDatasource = i_config.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(sqlDatasource))
            {
                // Қосылымды ашу
                connection.Open();

                // Параметрлермен сұрау орындау
                var affectedRows = connection.Execute(query, new
                {
                    fullname = user.Fullname,
                    email = user.Email,
                    password = user.Password,
                    roleId = user.roleId
                });

                // Қосылымды жабу (қосылымды жапқанда 'using' блок автоматты түрде жасалады)
                return Ok();
            }
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public JsonResult DeleteUser(int id)
        {
            string query = "delete from dbo.Users where id=@id";
            DataTable table = new DataTable();
            string sqlDatasource = i_config.GetConnectionString("DefaultConnection");
            SqlDataReader reader;
            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    mycon.Close();

                }
            }
            return new JsonResult("Deleted Successfully");
        }
        [HttpPost]
        [Route("Login")]
        public JsonResult Login([FromBody] Microsoft.AspNetCore.Identity.Data.LoginRequest request)
        {
            string query = "SELECT roleId FROM dbo.Users WHERE Email = @Email AND Password = @Password";
            string sqlDatasource = i_config.GetConnectionString("DefaultConnection");

            using (SqlConnection mycon = new SqlConnection(sqlDatasource))
            {
                mycon.Open();
                using (SqlCommand cmd = new SqlCommand(query, mycon))
                {
                    cmd.Parameters.AddWithValue("@Email", request.Email);
                    cmd.Parameters.AddWithValue("@Password", request.Password);

                    var role = cmd.ExecuteScalar();
                    if (role != null)
                    {
                        return new JsonResult(new { role = (int)role });
                    }
                    else
                    {
                        return new JsonResult("Invalid credentials") { StatusCode = 401 };
                    }
                }
            }
        }


    }
}
