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
        private readonly DapperContext context;

        public LoginRepository(DapperContext context)
        {
            //_dbContext = dBContext;
            this.context = context;
        }


        public async Task<DTOLogin> Login(DTOLogin login)
        {
            try
            {
                var query = @"
                SELECT TOP 1
                    LOGIN_IP,
                    NAME,
                    WORKING_BRANCH,
                    ALLOW_BR_SELECTION,
                    ini AS INI,
                    code AS CODE
                FROM UserMast
                WHERE ini = @INI AND code = @CODE
            ";

                using (var connection = context.CreateConnection())
                {
                    var result = await connection.QuerySingleOrDefaultAsync<DTOLogin>(query, new { INI = login.INI, CODE = login.CODE });
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
