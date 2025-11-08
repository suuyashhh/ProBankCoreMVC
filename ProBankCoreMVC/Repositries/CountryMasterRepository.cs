using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Net.WebSockets;

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

        public async Task Save(DTOCountryMaster country_list)
        {
            const string query = @"
        INSERT INTO MST_COUNTRY (TRN_NO, COUNTRY_NAME, DATE)
        VALUES (@CountryCode, @CountryName, @Date)
    ";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    long newCode = await GenerateCountryCode();

                    var parameters = new
                    {
                        CountryCode = newCode,
                        CountryName = country_list.COUNTRY_NAME,
                        Date = DateTime.Now
                    };

                    await connection.ExecuteAsync(query, parameters);
                }
            }
            catch
            {
                throw;
            }
        }



        private async Task<long> GenerateCountryCode()
        {
            const string query = "SELECT TOP 1 TRN_NO FROM MST_COUNTRY ORDER BY TRN_NO DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<string>(query);

                long nextNum = 1;

                if (!string.IsNullOrEmpty(lastId) && long.TryParse(lastId, out long lastNum))
                {
                    nextNum = lastNum + 1;
                }

                return nextNum;
            }
        }




    }
}
