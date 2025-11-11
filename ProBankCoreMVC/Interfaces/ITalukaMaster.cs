using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ITalukaMaster
    {
        Task<DTOTalukaMaster> GetTalukaById(int talukaCode);
    }
}
