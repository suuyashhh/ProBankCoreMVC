using Dapper;
using ProBankCoreMVC.Interfaces;
using ProBankCoreMVC.Contest; 
using System.Threading.Tasks;

namespace ProBankCoreMVC.Repositries
{
    public class LoginRepository : ILogin
    {
        private readonly DapperContext _context;

        public LoginRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUserAsync(string ini, string code)
        {
            var sql = "SELECT COUNT(1) FROM UserMast WHERE ini = @INI AND code = @CODE";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var count = await connection.ExecuteScalarAsync<int>(sql, new { INI = ini, CODE = code });
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error in ValidateUserAsync: {ex.Message}");
                throw;
            }
        }

    }
}