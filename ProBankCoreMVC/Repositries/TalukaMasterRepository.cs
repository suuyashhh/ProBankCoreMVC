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

        public async Task<DTOTalukaMaster> GetTalukaById(int talukaCode)
        {
            var query = @"
                            select
                            TRN_NO,TALUKA_NAME ,DIST_CODE
                            from MST_TALUKA where TRN_NO =@TalukaCode ";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryFirstOrDefaultAsync<DTOTalukaMaster>(query, new {TalukaCode = talukaCode });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
