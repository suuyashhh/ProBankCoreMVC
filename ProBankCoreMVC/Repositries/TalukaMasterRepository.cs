using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class TalukaMasterRepository : ITalukaMaster
    {
        private readonly DapperContext _dapperContext;

        public TalukaMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DTOTalukaMaster> GetTalukaById(int countryCode, int stateCode, int distCode, int talukaCode)
        {
            var query = @"
                            select
                            code AS TALUKA_CODE,name AS TALUKA_NAME 
                            from talkmast where Country_Code=@CountryCode AND State_Code= @StateCode AND Dist_Code = @DistCode AND code =@TalukaCode ";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOTalukaMaster>(query, new { CountryCode = countryCode, StateCode = stateCode, DistCode = distCode, TalukaCode = talukaCode });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
