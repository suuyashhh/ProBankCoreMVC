using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                  select Code,Name from CountryMast
                ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTOCountryMaster>(query);
            }
        }

        public async Task<DTOCountryMaster?> GetCountryById(long countryCode)
        {
            const string query = @"
                select Code, Name, Entry_Date from CountryMast
                WHERE Code = @CountryCode";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryFirstOrDefaultAsync<DTOCountryMaster>(query, new { CountryCode = countryCode });
            }
        }

        public async Task Save(DTOCountryMaster country)
        {
            const string query = @"
                INSERT CountryMast(Code, Name, Entry_Date)
                VALUES (@Code, @Name, @Entry_Date)";

            long newCode = await GenerateCountryCode();

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    Name = country.Name,
                    Entry_Date = DateTime.Now
                });
            }
        }

        public async Task Update(DTOCountryMaster country)
        {
            const string query = @"
                UPDATE CountryMast
                SET Name = @Name,
                    Entry_Date = @Entry_Date
                WHERE Code = @Code";


            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    country.Name,
                    Entry_Date = DateTime.Now,
                    country.Code
                });
            }
        }

        public async Task Delete(long countryCode)
        {
            const string query = @"DELETE FROM CountryMast WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { Code = countryCode });
            }
        }

        private async Task<long> GenerateCountryCode()
        {
            const string query = "SELECT TOP 1 Code FROM CountryMast ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query);
                return (lastId ?? 0) + 1;
            }
        }
    }
}
