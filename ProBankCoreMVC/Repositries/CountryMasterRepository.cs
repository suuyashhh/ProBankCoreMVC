using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class CountryMasterRepository : ICountryMaster
    {
        private readonly DapperContext _dapperContext;

        public CountryMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DTOCountryMaster?> GetCountryById(int countryCode)
        {
            var query = @"
        SELECT code AS COUNTRY_CODE, name AS COUNTRY_NAME
        FROM CountryMast
        WHERE code = @CountryCode";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOCountryMaster>(query, new { CountryCode = countryCode });
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
