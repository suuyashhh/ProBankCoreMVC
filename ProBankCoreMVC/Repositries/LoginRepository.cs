using Dapper;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System;
using System.Data;
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

        // Validate credentials - returns true if a matching user exists
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
                // consider using a logging framework instead of Console.WriteLine
                Console.WriteLine($"SQL Error in ValidateUserAsync: {ex.Message}");
                throw;
            }
        }

        // Store JTI in the user row (single-device enforcement)
        public async Task SetUserJtiAsync(string userId, string jti)
        {
            // Replace UserId with your actual PK column name if different
            var sql = "UPDATE UserMast SET CurrentJti = @Jti WHERE ini = @UserId";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(sql, new { Jti = jti, UserId = userId });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error in SetUserJtiAsync: {ex.Message}");
                throw;
            }
        }

        // Retrieve stored JTI for given user
        public async Task<string?> GetUserJtiAsync(string userId)
        {
            var sql = "SELECT CurrentJti FROM UserMast WHERE ini = @UserId";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<string>(sql, new { UserId = userId });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Error in GetUserJtiAsync: {ex.Message}");
                throw;
            }
        }
    }
}
