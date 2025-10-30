using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAreaMaster
    {
        Task<DTOAreaMaster> GetAreaById(int countryCode, int stateCode, int distCode, int talukaCode, int cityCode);
    }
}
