// ValidationServiceRepository.cs
using Dapper;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Repositries
{
    public class ValidationServiceRepository : IValidationService
    {
        private readonly DapperContext _dapperContext;
        public ValidationServiceRepository(DapperContext dapperContext) => _dapperContext = dapperContext;

        public async Task<bool> AadharNo(string aadharNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE AdharNo = @AadharNo;";

            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { AadharNo = aadharNo?.Trim() });

                return count > 0;
            }
        }
    }
}
