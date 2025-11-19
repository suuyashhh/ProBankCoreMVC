using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IPartyMaster
    {
        Task<string> Save(DTOPartyMaster p);
    }
}
