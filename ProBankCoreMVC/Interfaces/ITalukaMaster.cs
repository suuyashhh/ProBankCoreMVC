using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ITalukaMaster
    {
        Task<DTOTalukaMaster> GetTalukaById(int countryCode, int stateCode, int distCode, int talukaCode);
    }
}
