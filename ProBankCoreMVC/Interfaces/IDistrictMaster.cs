using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IDistrictMaster
    {
        Task<DTODistrictMaster> GetDistrictById(int distCode);
    }
}
