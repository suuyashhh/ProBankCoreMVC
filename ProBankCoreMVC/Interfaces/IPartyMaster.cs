using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IPartyMaster
    {
        Task save(DTOPartyMaster partymaster);
    }
}
