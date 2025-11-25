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
                u.LOGIN_IP,
	            u.user_level AS USER_LAVEL,
                u.NAME,
                u.WORKING_BRANCH,
                u.ALLOW_BR_SELECTION,
                u.ini AS INI,
                u.code AS CODE,
	            ug.name AS DESIGNATION
                FROM UserMast AS u
                JOIN UserGrade AS ug
                ON ug.Code = u.user_level
                WHERE u.ini = @INI AND u.code = @CODE
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
