using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAgentMaster
    {
        Task<IEnumerable<DTOAgentMaster>> GetAllAgent();
        Task<object> GetAgentById(int ID);
        Task Save(DTOAgentMaster Agent);
        Task Update(DTOAgentMaster Agent);
        Task Delete(int ID);

    }
}
