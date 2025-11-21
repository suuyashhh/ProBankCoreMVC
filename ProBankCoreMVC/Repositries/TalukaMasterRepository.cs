using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class TalukaMasterRepository : ITalukaMaster
    {
        private readonly DapperContext _dapperContext;

        public TalukaMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<DTOTalukaMaster> GetTalukaById(int talukaCode, int Dist_code, int State_Code, int Country_Code)
        {
            var query = @"
                        select 
                        t.Code, t.Name, t.Dist_code, t.Country_Code, t.State_Code , d.Name AS District_Name , s.Name AS State_Name, c.Name AS Country_Name
                        from talkmast AS t
                        JOIN CountryMast AS c 
                        ON c.Code = t.Country_Code
                        JOIN StateMast AS s
                        ON s.Code = t.State_Code AND s.Country_Code = t.Country_Code
                        JOIN DistrictMast AS d
                        ON d.Code=t.Dist_code AND d.Country_Code = t.Country_Code AND d.State_Code = t.State_Code
                        where t.Code = @TalukaCode AND t.Country_Code=@CountryCode AND t.State_Code=@StateCode AND t.Dist_code=@DistrictCode";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOTalukaMaster>(query, new { TalukaCode = talukaCode,DistrictCode=Dist_code,StateCode=State_Code,CountryCode=Country_Code });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IEnumerable<DTOTalukaMaster>> GetTaluka(int Dist_code, int State_Code, int Country_Code)
        {
            var query = @"select
                             Code, Name, Dist_code, Country_Code, State_Code
                            from talkmast where Dist_code = @Dist_code and Country_Code = @Country_Code and State_Code = @State_Code ";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOTalukaMaster>(query, new { Dist_code = Dist_code, State_Code = State_Code, Country_Code = Country_Code,});
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public async Task<IEnumerable<DTOTalukaMaster>> GetAll()
        {
            const string query = @"
                 select 
                        t.Code, t.Name, t.Dist_code, t.Country_Code, t.State_Code , d.Name AS District_Name , s.Name AS State_Name, c.Name AS Country_Name
                        from talkmast AS t
                        JOIN CountryMast AS c 
                        ON c.Code = t.Country_Code
                        JOIN StateMast AS s
                        ON s.Code = t.State_Code AND s.Country_Code = t.Country_Code
                        JOIN DistrictMast AS d
                        ON d.Code=t.Dist_code AND d.Country_Code = t.Country_Code AND d.State_Code = t.State_Code
                ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                return await conn.QueryAsync<DTOTalukaMaster>(query);
            }
        }

        public async Task Save(DTOTalukaMaster taluka)
        {
            const string query = @"
                INSERT INTO talkmast (Code, Country_Code, name, mname, Entry_Date)
                VALUES (@Code, @Country_Code, @Name, @mname, @Entry_Date)";

            long newCode = await GenerateTalukaCode(taluka.Country_Code, taluka.State_Code, taluka.Dist_code);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    Country_Code = taluka.Country_Code,
                    mname = taluka.mname,
                    name = taluka.name,
                    Entry_Date = DateTime.Now
                });
            }
        }

        public async Task Update(DTOTalukaMaster taluka)
        {
            const string query = @"
                  UPDATE talkmast
                         SET name = @name,
	                         Dist_code = @Dist_code,
	                         State_Code = @State_Code,
	                         Country_Code = @Country_Code,
	                         mname = @mname,
                             Entry_Date = @Entry_Date
                         WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    taluka.name,
                    Dist_code = taluka.Dist_code,
                    Country_Code = taluka.Country_Code,
                    mname = taluka.mname,
                    State_Code = taluka.State_Code,
                    Entry_Date = DateTime.Now,
                    taluka.Code
                });
            }
        }

        public async Task Delete(long Code, int Dist_code, int State_Code, int Country_Code)
        {
            const string query = @"DELETE FROM talkmast WHERE Code = @Code and Dist_code = @Dist_code and State_Code = @State_Code and Country_Code = @Country_Code ";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { Code = Code, Dist_code = Dist_code, State_Code = State_Code, Country_Code = Country_Code });
            }
        }

        private async Task<long> GenerateTalukaCode(int Country_Code, int State_Code, int Dist_code)
        {
            const string query = "SELECT TOP 1 Code FROM talkmast WHERE Country_Code=@CountryCode AND State_Code=@StateCode AND Dist_code=@Distcode ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { CountryCode= Country_Code, StateCode = State_Code, Distcode= Dist_code });
                return (lastId ?? 0) + 1;
            }
        }
    }
}
