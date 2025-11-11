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

        public async Task<DTODistrictMaster> GetDistrictById(int distCode)
        {
            var query = @"
                        select 
                            TRN_NO,DIST_NAME,STATE_CODE
                            from MST_DISTRICT where TRN_NO = @distCode ";
            try
            {
                using (var Connection = _dapperContext.CreateConnection()) 
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTODistrictMaster>(query, new { DistCode = distCode });
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
