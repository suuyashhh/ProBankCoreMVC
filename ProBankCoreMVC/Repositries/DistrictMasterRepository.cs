using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class DistrictMasterRepository : IDistrictMaster
    {
        private readonly DapperContext _dapperContext;

        public DistrictMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DTODistrictMaster> GetDistrictById(int countryCode , int stateCode ,int distCode)
        {
            var query = @"
                        select 
                            code AS DIST_CODE,name AS DIST_NAME
                            from DistrictMast where Country_Code=@CountryCode AND State_Code= @StateCode AND code = @distCode ";
            try
            {
                using (var Connection = _dapperContext.CreateConnection()) 
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTODistrictMaster>(query, new { CountryCode = countryCode, StateCode = stateCode, DistCode = distCode });
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
