using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class KycAddressMasterRepository : IKycAddressMaster
    {
        private readonly DapperContext _dapperContext;

        public KycAddressMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOKycAddressMaster>> GetAllKycAddress()
        {
            var query = @"
                            select CODE AS KYC_ADDR_CODE,NAME AS KYC_ADDR_NAME from KycAddrmast";
            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOKycAddressMaster>(query);
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
