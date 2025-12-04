using Microsoft.AspNetCore.Mvc;
using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IStaffMaster
    {
        Task<IEnumerable<DTOStaffMaster>> GetAllStaff();
        Task<DTOStaffMaster> GetStaffById(int code);
        Task<IEnumerable<DTOStaffMaster>> GetStaff(int code);
        Task Save(DTOStaffMaster Staff);
        Task Update(DTOStaffMaster Staff);
        Task Delete(long code);
    }
}
