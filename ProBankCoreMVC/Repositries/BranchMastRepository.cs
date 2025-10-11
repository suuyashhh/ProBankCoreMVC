using Dapper;
using Models;
using ProBankCoreMVC.Contest; // keep this if your DapperContext is in the Contest namespace
using ProBankCoreMVC.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace ProBankCoreMVC.Repositries
{
    public class BranchMastRepository : IBranchMast
    {
        private readonly DapperContext _dapperContext;

        public BranchMastRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<IEnumerable<DTOBranchMast>> GetAllBranches()
        {
            var query = @"
                SELECT 
                    code AS CODE,
                    name AS NAME
                FROM brncmast;
            ";

            try
            {
                using (var connection = _dapperContext.CreateConnection())
                {
                    var result = await connection.QueryAsync<DTOBranchMast>(query);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
