using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Repositries
{
    public class CountryMasterRepository : ICountryMaster
    {
        private readonly DapperContext _dapperContext;

        public CountryMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOCountryMaster>> GetAll()
        {
            const string query = @"
                SELECT TRN_NO, COUNTRY_NAME, DATE
                FROM MST_COUNTRY
                ORDER BY TRN_NO DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTOCountryMaster>(query);
            }
        }

        public async Task<DTOCountryMaster?> GetCountryById(long countryCode)
        {
            const string query = @"
                SELECT TRN_NO, COUNTRY_NAME, DATE
                FROM MST_COUNTRY
                WHERE TRN_NO = @CountryCode";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DTOCountryMaster>(query, new { CountryCode = countryCode });
            }
        }

        public async Task Save(DTOCountryMaster country)
        {
            const string query = @"
                INSERT INTO MST_COUNTRY (TRN_NO, COUNTRY_NAME, DATE)
                VALUES (@TRN_NO, @COUNTRY_NAME, @DATE)";

            long newCode = await GenerateCountryCode();

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    TRN_NO = newCode,
                    COUNTRY_NAME = country.COUNTRY_NAME,
                    DATE = DateTime.Now
                });
            }
        }

        public async Task Update(DTOCountryMaster country)
        {
            const string query = @"
                UPDATE MST_COUNTRY
                SET COUNTRY_NAME = @COUNTRY_NAME,
                    DATE = @DATE
                WHERE TRN_NO = @TRN_NO";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    country.COUNTRY_NAME,
                    DATE = DateTime.Now,
                    country.TRN_NO
                });
            }
        }

        public async Task Delete(long countryCode)
        {
            const string query = @"DELETE FROM MST_COUNTRY WHERE TRN_NO = @CountryCode";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { CountryCode = countryCode });
            }
        }

        private async Task<long> GenerateCountryCode()
        {
            const string query = "SELECT TOP 1 TRN_NO FROM MST_COUNTRY ORDER BY TRN_NO DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query);
                return (lastId ?? 0) + 1;
            }
        }
    }
}
