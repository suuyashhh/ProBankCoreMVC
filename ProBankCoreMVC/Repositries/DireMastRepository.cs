using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class DireMastRepository : IDireMast
    {
        private readonly DapperContext _dapperContext;

        public DireMastRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTODireMast>> GetAllOther()
        {
            var query = @"
                            SELECT code as OTHER_CODE , NAME AS OTHER_NAME FROM diremast";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTODireMast>(query);
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
