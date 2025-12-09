using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class AccountTypeMasterRepository : IAccountTypeMaster
    {

        private readonly DapperContext _dapperContext;

        public AccountTypeMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOAccountTypeMaster>> GetAllAccountType()
        {
            var query = @"select  Code, Name, Allow_Mobile_App, Allow_Mobile_App_Trn,AdharCard,PanCard,GST  from Party_AcTypeMast";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOAccountTypeMaster>(query);
                    return result.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DTOAccountTypeMaster> GetAccountTypeById(int Code)
        {
            var query = @"select  Code, Name, Allow_Mobile_App, Allow_Mobile_App_Trn,AdharCard,PanCard,GST from Party_AcTypeMast where Code = @Code";

            try
            {
                using (var Connection = _dapperContext.CreateConnection())
                {
                    var result = await Connection.QueryFirstOrDefaultAsync<DTOAccountTypeMaster>(query, new { Code = Code });

                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Save(DTOAccountTypeMaster AccountType)
        {
            const string query = @"  INSERT INTO Party_AcTypeMast (Code, Name, Allow_Mobile_App, Allow_Mobile_App_Trn )  VALUES (@Code, @Name, @Allow_Mobile_App, @Allow_Mobile_App_Trn )";


            long newCode = await GenerateAccountTypeCode(AccountType.Code);

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new
                {

                    Code = newCode,
                    Name = AccountType.Name,
                    Allow_Mobile_App = AccountType.Allow_Mobile_App,
                    Allow_Mobile_App_Trn = AccountType.Allow_Mobile_App_Trn
                });
            }
        }

        public async Task Update(DTOAccountTypeMaster AccountType)
        {
            const string query = @"UPDATE Party_AcTypeMast SET Name = @Name, Allow_Mobile_App = @Allow_Mobile_App, Allow_Mobile_App_Trn = @Allow_Mobile_App_Trn  WHERE Code = @Code";

            try
            {
                using (var conn = _dapperContext.CreateConnection())
                {
                    await conn.ExecuteAsync(query, new
                    {
                        Code = AccountType.Code,
                        Name = AccountType.Name,
                        Allow_Mobile_App = AccountType.Allow_Mobile_App,
                        Allow_Mobile_App_Trn = AccountType.Allow_Mobile_App_Trn
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
            const string query = @"DELETE FROM Party_AcTypeMast WHERE Code = @Code";

            using (var conn = _dapperContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { Code = Code, });
            }
        }

        private async Task<long> GenerateAccountTypeCode(int code)
        {
            const string query = "SELECT TOP 1 Code FROM Party_AcTypeMast ORDER BY Code DESC";

            using (var conn = _dapperContext.CreateConnection())
            {
                var lastId = await conn.ExecuteScalarAsync<long?>(query, new { code = code });
                return (lastId ?? 0) + 1;
            }
        }
    }
}
