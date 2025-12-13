using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class DesignationRepository:IDesignationMaster
    {
        private readonly DapperContext _dapperContext;

        public DesignationRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTODesignationMaster>> GetAllDesignations()
        {
            var query = @"SELECT code, designation FROM Designation_Master;";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTODesignationMaster>(query);
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
