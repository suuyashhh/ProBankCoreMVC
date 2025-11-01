using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class OccupationMasterRepository : IOccupationMaster
    {
        private readonly DapperContext _dapperContext;
        public OccupationMasterRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOOccupationMaster>> GetAllOccupations()
        {
            var query = @"
                            select code AS OCCUP_CODE,name AS OCCUP_NAME from occumast";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOOccupationMaster>(query);
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
