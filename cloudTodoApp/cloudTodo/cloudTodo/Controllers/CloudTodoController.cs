using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace cloudTodo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudTodoController : ControllerBase
    {
        private IConfiguration _configuration;
        public CloudTodoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetNotes")]

        public JsonResult GetNotes()
        {
            string query = "Select * from dbo.notes";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("dbcon");
            SqlDataReader myReader;
            using(SqlConnection myCon=new SqlConnection(sqlDatasource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon)) {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                
                }
            }
            return new JsonResult(table);
        }
    }
}
