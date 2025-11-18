using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class StateMasterRepository : IStateMaster
    {
        private readonly DapperContext _dapperContext;

        public StateMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public async Task<IEnumerable<DTOStateMaster>> GetAll()
        {
            const string query = @"
                 SELECT
                      s.Code,
                      s.Name,
                      s.Country_Code,
                      c.Name AS Country_Name
                    FROM StateMast AS s
                    JOIN CountryMast AS c
                      ON c.Code = s.Country_Code
                    ORDER BY s.Code DESC;";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTOStateMaster>(query);
            }
        }

        public async Task Save(DTOStateMaster State_List)
        {
            const string query = @"
                INSERT StateMast(Code, Name,Country_Code)
                VALUES (@Code, @Name,@Country_Code)";
            var countryCode = State_List.Country_Code;
            long newCode = await GenerateStateCode(countryCode);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    Name = State_List.Name,
                    Country_Code = State_List.Country_Code
                });
            }
        }

        public async Task Update(DTOStateMaster State_List)
        {
            const string query = @"
                UPDATE StateMast
                SET Name = @Name
                WHERE Code = @Code";


            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    State_List.Name,
                    State_List.Code
                });
            }
        }

        public async Task Delete(long stateCode, int countryCode)
        {
            const string query = @"DELETE FROM StateMast WHERE Code = @Code and Country_Code = @Country_Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { Code = stateCode, Country_Code = countryCode });
            }
        }


        public async Task<DTOStateMaster?> GetStateById(int stateCode, int countryCode)
        {
            var query = @"            
                            select 
                            s.Code,s.Name,s.Country_Code,c.Name AS Country_Name
                            from StateMast AS s
                            JOIN CountryMast AS c
                            ON c.Code = s.Country_Code
                            WHERE  s.Code = @StateCode and s.Country_Code = @Country_Code";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOStateMaster>(query,
                        new { StateCode = stateCode, Country_Code = countryCode });

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DTOStateMaster>> GetState( int countryCode)
        {
            var query = @"
            select Code,Name,Country_Code from StateMast
            WHERE  Country_Code = @Country_Code";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOStateMaster>(query,
                        new { Country_Code = countryCode });

                    return result.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<long> GenerateStateCode(int countryCode)
        {
            const string query = "SELECT TOP 1 Code FROM StateMast where Country_Code= @Country_Code ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { Country_Code = countryCode });
                return (lastId ?? 0) + 1;
            }
        }

    }







}
