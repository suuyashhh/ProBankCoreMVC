// ValidationServiceRepository.cs
using Dapper;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using System.Reflection;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Repositries
{
    public class ValidationServiceRepository : IValidationService
    {
        private readonly DapperContext _dapperContext;
        public ValidationServiceRepository(DapperContext dapperContext) => _dapperContext = dapperContext;

        public async Task<bool> AadharNo(string aadharNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE AdharNo = @AadharNo";

            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { AadharNo = aadharNo});

                return count > 0;
            }
        }

        public async Task<bool> PanNo(string panNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE pan_no = @PanNo";

            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { PanNo = panNo});

                return count > 0;
            }
        }

        public async Task<bool> GstNo(string gstNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE GST_No = @GstNo";
            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { GstNo = gstNo});
               return count > 0;
            }


        }

        public async Task<bool> MobileNo(string mobileNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE Mobile = @mob_No";
            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { mob_No = mobileNo });
                return count > 0;
            }


        }

        public async Task<bool> PhoneNo(string phone1)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE PHONE1 = @PhoneNo";
            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { PhoneNo = phone1 });
                return count > 0;
            }


        }
        public async Task<bool> VoterIdNo(string voterIdNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE VOTERIDNO = @voterIdNo";
            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { VoterIdNo = voterIdNo });
                return count > 0;
            }


        }

        public async Task<bool> PassportNo(string passportNo)
        {
            var sql = @"SELECT COUNT(1) FROM prtymast WHERE PASSPORTNO = @passportNo";
            using (var con = _dapperContext.CreateConnection())
            {
                var count = await con.ExecuteScalarAsync<long>(sql, new { PassportNo = passportNo });
                return count > 0;
            }


        }
        

    }
}
