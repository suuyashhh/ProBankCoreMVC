using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAgentMaster
    {
        Task<IEnumerable<DTOAgentMaster>> GetAllAgent();
        Task<object> GetAgentById(int ID);
        Task<long> Save(DTOAgentMaster Agent);
        Task Update(DTOAgentMaster Agent);
        Task Delete(int ID);

    }
}
