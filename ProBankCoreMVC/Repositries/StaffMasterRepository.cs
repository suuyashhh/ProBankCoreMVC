using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class StaffMasterRepository : IStaffMaster
    {
        private readonly DapperContext _dapperContext;

        public StaffMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOStaffMaster>> GetAllStaff()
        {
            var query = @"SELECT code,name,Entry_Date  FROM StaffMast";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOStaffMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DTOStaffMaster> GetStaffById(int code)
        {
            var query = @"select name,mname,code,Entry_Date from StaffMast where code = @code";

            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOStaffMaster>(query, new { code = code});

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DTOStaffMaster>> GetStaff(int code)
        {
            var query = @"SELECT code,name,Entry_Date  FROM StaffMast  where code = @code";
            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryAsync<DTOStaffMaster>(query, new { code});
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Save(DTOStaffMaster Staff)
        {
            const string query = @"INSERT INTO StaffMast (code, name, Entry_Date)  VALUES (@code, @name, @Entry_Date)";


            long newCode = await GenerateStaffCode(Staff.code);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {
                    Code = newCode,
                    name = Staff.name,
                    Entry_Date = DateTime.Now


                });
            }
        }

        public async Task Update(DTOStaffMaster Staff)
        {
            const string query = @" UPDATE StaffMast SET name = @name WHERE code = @code";

            try
            {
                using (var conn = _dapperContext.CreateConnection())
                {
                    await conn.ExecuteAsync(query, new
                    {
                        Staff.name,
                        Entry_Date = DateTime.Now,
                        Staff.code
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
            const string query = @"DELETE FROM StaffMast WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { code = code,});
            }
        }

        private async Task<long> GenerateStaffCode(int code)
        {
            const string query = "SELECT TOP 1 Code FROM staffMast ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { code = code });
                return (lastId ?? 0) + 1;
            }
        }


    }
}
