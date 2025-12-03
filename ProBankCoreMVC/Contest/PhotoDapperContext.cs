using Microsoft.Data.SqlClient;
using System.Data;

namespace ProBankCoreMVC.Contest
{
    public class PhotoDapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public PhotoDapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connPhotoSign");
        }
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
