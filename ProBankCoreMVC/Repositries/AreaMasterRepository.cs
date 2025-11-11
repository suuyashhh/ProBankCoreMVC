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

        public async Task<IEnumerable<DTOAreaMaster>> GetAreaById(int countryCode, int stateCode, int distCode, int talukaCode, int cityCode) 
        {
            var query = @"
                            select
                            code AS AREA_CODE,name AS AREA_NAME,PinCode AS PIN_CODE
                            from AreaMast where CountryCode=@CountryCode AND StateCode= @StateCode AND DistCode = @DistCode AND TalukaCode =@TalukaCode AND CityCode =@CityCode ";
            try
            {
                using (var connection = _dapperContext.CreateConnection()) 
                {

                    var result = await connection.QueryAsync<DTOAreaMaster>(query, new { CountryCode = countryCode, StateCode = stateCode, DistCode = distCode, TalukaCode = talukaCode, CityCode= cityCode });
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
