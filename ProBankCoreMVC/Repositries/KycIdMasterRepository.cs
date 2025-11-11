using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class KycIdMasterRepository : IKycIdMaster
    {
        private readonly DapperContext _dapperContext;

        public KycIdMasterRepository(DapperContext dapperContext)
        {

            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOKycIdMaster>> GetAllKycId()
        {
            var query = @"
                            select CODE AS KYC_ID_CODE,NAME AS KYC_ID_NAME from KycIdMast";
            try
            {
                using (var connection = _dapperContext.CreateConnection()) 
                {
                    var result= await connection.QueryAsync<DTOKycIdMaster>(query);
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
