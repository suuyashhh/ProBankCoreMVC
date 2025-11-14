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

        public async Task<DTODistrictMaster> GetDistrictById(int distCode, int Country_Code, int State_Code)
        {
            var query = @"
                       select Code,State_Code,Country_Code,Name,Entry_Date from DistrictMast
                             where Code = @distCode";
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

        public async Task<IEnumerable<DTODistrictMaster>> GetAll()
        {
            const string query = @"
                  select Code,State_Code,Country_Code,Name,Entry_Date from DistrictMast
                ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTODistrictMaster>(query);
            }
        }

        public async Task Save(DTODistrictMaster objList)
        {
            const string query = @"
                INSERT DistrictMast(Code, State_Code , Country_Code, Name, Entry_Date)
                VALUES ( @Code, @State_Code, @Country_Code, @Name, @Entry_Date)";


            long newCode = await GenerateDistrictCode();

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    State_Code = objList.State_Code,
                    Country_Code = objList.Country_Code,
                    Name = objList.Name,
                    Entry_Date = DateTime.Now


                });
            }
        }

        public async Task Update(DTODistrictMaster district)
        {
            const string query = @"
                UPDATE DistrictMast
                SET Name = @Name,
                    Entry_Date = @Entry_Date,
                    State_Code = @State_Code,
                    Country_Code = @Country_Code
                WHERE Code = @Code";


            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    district.Name,
                    Entry_Date = DateTime.Now,
                    State_Code = district.State_Code,
                    Country_Code = district.Country_Code,
                    district.Code
                });
            }
        }

        public async Task Delete(long Code, int State_Code, int Country_Code)
        {
            const string query = @"DELETE FROM DistrictMast WHERE Code = @Code and State_Code = @State_Code and Country_Code = @Country_Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { Code = Code, State_Code = State_Code, Country_Code = Country_Code });
            }
        }

        private async Task<long> GenerateDistrictCode()
        {
            const string query = "SELECT TOP 1 Code FROM DistrictMast  ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query);
                return (lastId ?? 0) + 1;
            }
        }



    }
}
