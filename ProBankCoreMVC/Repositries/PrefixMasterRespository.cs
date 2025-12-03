using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class PrefixMasterRespository : IPrefixMaster
    {
        private readonly DapperContext _dapperContext;

        public PrefixMasterRespository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOPrefixMaster>> GetAllPrefix()
        {
            var query = @"SELECT Code,Prefixtype From PrefixMast";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOPrefixMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public async Task<DTOPrefixMaster> GetPrefixById(int code)
        {
            var query = @"SELECT Code,Prefixtype From PrefixMast where code = @code";

            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOPrefixMaster>(query, new { code = code });

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task<IEnumerable<DTOStaffMaster>> GetStaff(int code)
        //{
        //    var query = @"SELECT code,name,Entry_Date  FROM StaffMast  where code = @code";
        //    try
        //    {
        //        using (var Connection = _dapperContext.CreateConnection())
        //        {
        //            var result = await Connection.QueryAsync<DTOStaffMaster>(query, new { code });
        //            return result;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async Task Save(DTOPrefixMaster Prefix)
        {
            const string query = @"INSERT INTO PrefixMast (Code, PrefixType) VALUES ( @Code, @PrefixType);";


            long newCode = await GeneratePrefixCode(Prefix.Code);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    Prefix.Prefixtype
                });
            }
        }

        public async Task Update(DTOPrefixMaster Prefix)
        {
            const string query = @" UPDATE PrefixMast SET Prefixtype = @Prefixtype WHERE Code = @Code";

            try
            {
                using (var conn = _dapperContext.CreateConnection())
                {
                    await conn.ExecuteAsync(query, new
                    {
                        Prefix.Code,
                        Prefix.Prefixtype
                    });
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(long Code)
        {
            const string query = @"DELETE FROM PrefixMast WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { code = Code, });
            }
        }

        private async Task<long> GeneratePrefixCode(int code)
        {
            const string query = "SELECT TOP 1 Code FROM PrefixMast ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { code = code });
                return (lastId ?? 0) + 1;
            }
        }



    }
}
