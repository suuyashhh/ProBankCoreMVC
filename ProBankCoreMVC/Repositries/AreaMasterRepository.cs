using Dapper;
using Microsoft.Identity.Client;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class AreaMasterRepository : IAreaMaster
    {
        private readonly DapperContext _dapperContext;

        public AreaMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext; 
        }

        public async Task<IEnumerable<DTOAreaMaster>> GetAreaById(int cityCode) 
        {
            var query = @"
                            select
                            TRN_NO AS AREA_CODE,AREA_NAME,PIN_CODE
                            from MST_AREA where CITY_CODE =@CityCode ";
            try
            {
                using (var connection = _dapperContext.CreateConnection()) 
                {

                    var result = await connection.QueryAsync<DTOAreaMaster>(query, new { CityCode= cityCode });
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
