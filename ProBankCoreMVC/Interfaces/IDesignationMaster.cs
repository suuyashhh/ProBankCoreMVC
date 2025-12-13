using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IDesignationMaster
    {
        Task<IEnumerable<DTODesignationMaster>> GetAllDesignations();
    }
}
