using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface IBranchMast
    {
        Task<IEnumerable<DTOBranchMast>> GetAllBranches();
    }
}
