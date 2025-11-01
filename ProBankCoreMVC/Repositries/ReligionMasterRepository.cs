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
                            select code As RELIGION_CODE,name AS RELIGION_NAME from ReligionMast";

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
    }
}
