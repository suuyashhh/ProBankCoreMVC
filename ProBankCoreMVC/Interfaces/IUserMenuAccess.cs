// ProBankCoreMVC/Interfaces/IUserMenuAccess.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IUserMenuAccess
    {
        Task<IEnumerable<DTOUserGrade>> GetUserGradesAsync();
        Task<IEnumerable<DTOMenuMasterItem>> GetMenuMasterAsync(int programeId);
        Task<IEnumerable<long>> GetSelectedMenuIdsAsync(long userGrad, int programeId);
        Task SaveUserMenuAccessAsync(DTOUserMenuAccess model);
    }
}
