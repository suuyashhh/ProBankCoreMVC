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

        public async Task<DTOStateMaster?> GetStateById( int stateCode)
        {
            var query = @"
        SELECT
            TRN_NO,STATE_NAME,COUNTRY_CODE
        FROM StateMast
        WHERE  TRN_NO = @StateCode";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOStateMaster>(query,
                        new { StateCode = stateCode });

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
