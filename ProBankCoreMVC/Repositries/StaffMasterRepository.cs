using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class StaffMasterRepository :IStaffMaster
    {
        private readonly DapperContext _dapperContext;

        public StaffMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOStaffMaster>> GetAllStaff()
        {
            var query = @"
                            SELECT CODE AS STAFF_CODE,NAME AS STAFF_NAME  FROM StaffMast";
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
    }
}
