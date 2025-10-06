using Dapper;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using Models;
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

        // Query the UserMast table for a matching user and return details
        public async Task<DTOLogin?> LoginAsync(DTOLogin login)
        {
            var sql = @"
                SELECT TOP 1
                    LOGIN_IP,
                    NAME,
                    AUTHORITY,
                    ACTIVATE,
                    ini AS INI,
                    code AS CODE
                FROM UserMast
                WHERE ini = @INI AND code = @CODE
            ";

            try
            {
                using (var connection = _context.CreateConnection())
                {
                    // QuerySingleOrDefaultAsync will return null when no match
                    var user = await connection.QuerySingleOrDefaultAsync<DTOLogin>(sql, new { INI = login.INI, CODE = login.CODE });
                    return user;
                }
            }
            catch (Exception ex)
            {
                // Consider logging
                Console.WriteLine($"SQL Error in LoginAsync: {ex.Message}");
                throw;
            }
        }

        // Store JTI in the user row (single-device enforcement)
        public async Task SetUserJtiAsync(string userId, string jti)
        {
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

        // Retrieve stored JTI for given user (by user id or ini)
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
