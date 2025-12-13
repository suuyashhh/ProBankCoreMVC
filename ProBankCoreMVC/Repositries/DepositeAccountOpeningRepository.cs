using Dapper;
using Models;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;

namespace ProBankCoreMVC.Repositries
{
    public class DepositeAccountOpeningRepository: IDepositeAccountOpening
    {
        private readonly DapperContext _dapperContext;

        public DepositeAccountOpeningRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTODepositeAccountOpening>> GetGlCodeAll()
        {
            var query = @"
                            select [type],code,name,* from glmaster where [group]='D' and subsi='Y'
                            ";
            try
            {
                using (var con = _dapperContext.CreateConnection()) 
                {
                    var result= await con.QueryAsync<DTODepositeAccountOpening>(query);
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
