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
                        select 
                        d.Code, d.State_Code,d.Country_Code,d.Name,d.Entry_Date,c.Name as Country_Name,s.Name AS State_Name 
                        from DistrictMast AS d
                        JOIN CountryMast AS c
                        ON c.Code = d.Country_Code 
                        JOIN StateMast AS s
                        ON s.Code = d.State_Code AND s.Country_Code = d.Country_Code
                        where d.Code = @distCode AND d.Country_Code=@CountryCode AND d.State_Code=@StateCode";
            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTODistrictMaster>(query, new { DistCode = distCode, CountryCode = Country_Code, StateCode = State_Code });
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DTODistrictMaster>> GetDistrict(int Country_Code, int State_Code)
        {
            var query = @"
                       select State_Code, Name, Country_Code from DistrictMast
                             where State_Code = @State_Code and Country_Code = @Country_Code ";
            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryAsync<DTODistrictMaster>(query, new { Country_Code = Country_Code, State_Code = State_Code });
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
                      select 
                      d.Code,d.State_Code,d.Country_Code,d.Name,c.Name AS Country_Name,s.Name AS State_Name, d.Entry_Date
                      from DistrictMast AS d
                      JOIN CountryMast  AS c 
                      ON c.Code = d.Country_Code
                      JOIN StateMast AS s
                      ON s.code = d.State_Code
                    ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTODistrictMaster>(query);
            }
        }

        public async Task Save(DTODistrictMaster objList)
        {
            const string query = @"
    INSERT INTO DistrictMast (Code, State_Code, Country_Code, Name, Entry_Date)
    VALUES (@Code, @State_Code, @Country_Code, @Name, @Entry_Date)";


            long newCode = await GenerateDistrictCode(objList.Country_Code, objList.State_Code);

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
                    Entry_Date = @Entry_Date
                WHERE Code = @Code AND Country_Code = @Country_Code AND State_Code = @State_Code ";

            try
            {
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
            catch (Exception)
            {
                throw;
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

        private async Task<long> GenerateDistrictCode(int Country_Code, int State_Code)
        {
            const string query = "SELECT TOP 1 Code FROM DistrictMast WHERE Country_Code=@CountryCode AND State_Code=@StateCode  ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { CountryCode = Country_Code, StateCode = State_Code });
                return (lastId ?? 0) + 1;
            }
        }



    }
}
