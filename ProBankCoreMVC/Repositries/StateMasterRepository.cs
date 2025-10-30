using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class StateMasterRepository : IStateMaster
    {
        private readonly DapperContext _dapperContext;

        public StateMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DTOStateMaster?> GetStateById(int countryCode, int stateCode)
        {
            var query = @"
        SELECT
            code AS STATE_CODE,
            name AS STATE_NAME
        FROM StateMast
        WHERE Country_Code = @CountryCode AND code = @StateCode";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOStateMaster>(query,
                        new { CountryCode = countryCode, StateCode = stateCode });

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
