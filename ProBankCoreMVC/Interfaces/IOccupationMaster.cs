using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IOccupationMaster
    {
        Task<IEnumerable<DTOOccupationMaster>> GetAllOccupations();
    }
}
