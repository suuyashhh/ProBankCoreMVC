using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IStaffMaster
    {
        Task<IEnumerable<DTOStaffMaster>> GetAllStaff();
    }
}
