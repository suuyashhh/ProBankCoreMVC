using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IReligionMaster
    {
        Task<IEnumerable<DTOReligionMaster>> GetAllReligion();
    }
}
