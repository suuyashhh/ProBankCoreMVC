using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAgentMaster
    {
        Task<IEnumerable<DTOAgentMaster>> GetAllAgent();

    }
}
