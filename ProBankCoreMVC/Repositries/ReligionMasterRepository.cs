using Dapper;
using Microsoft.AspNetCore.Routing.Constraints;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class ReligionMasterRepository: IReligionMaster
    {
        private readonly DapperContext _dapperContext;

        public ReligionMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOReligionMaster>> GetAllReligion()
        {
            var query = @"
                            select code As RELIGION_CODE,name AS RELIGION_NAME, Entry_Date AS RELIGION_DATE from ReligionMast";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOReligionMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DTOReligionMaster> GetReligionById(int code)
        {
            var query = @"select Name AS RELIGION_NAME , Code As RELIGION_CODE, Entry_Date AS RELIGION_DATE from ReligionMast where code = @code";

            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOReligionMaster>(query, new { code = code });

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Save(DTOReligionMaster Religion)
        {
            const string query = @"INSERT INTO ReligionMast (Code, Name, Entry_Date)  VALUES (@code, @name, @Entry_Date)";


            long newCode = await GenerateReligionCode(Religion.RELIGION_CODE);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    Name = Religion.RELIGION_NAME,
                    Entry_Date = DateTime.Now


                });
            }
        }

        private async Task<long> GenerateReligionCode(int RELIGION_CODE)
        {
            const string query = "SELECT TOP 1 Code FROM ReligionMast ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { code = RELIGION_CODE });
                return (lastId ?? 0) + 1;
            }
        }


        public async Task Update(DTOReligionMaster Religion)
        {
            const string query = @" UPDATE ReligionMast SET Name = @Name WHERE Code = @Code ";

            try
            {
                using (var conn = _dapperContext.CreateConnection())
                {
                    await conn.ExecuteAsync(query, new
                    {
                        Name= Religion.RELIGION_NAME,
                        Entry_Date = DateTime.Now,
                        Code = Religion.RELIGION_CODE
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(long code)
        {
            const string query = @"DELETE FROM ReligionMast WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { code = code, });
            }
        }

    }
}
