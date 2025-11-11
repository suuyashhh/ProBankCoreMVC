using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IStateMaster
    {
        Task<DTOStateMaster> GetStateById(int stateCode);
    }
}
