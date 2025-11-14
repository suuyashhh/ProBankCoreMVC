using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IStateMaster
    {
        Task<DTOStateMaster> GetStateById(int stateCode, int countryCode);
        Task<IEnumerable<DTOStateMaster>> GetAll();
        Task Save(DTOStateMaster State_List);
        Task Update(DTOStateMaster State_List);
        Task Delete(long stateCode, int countryCode);

    }
}
