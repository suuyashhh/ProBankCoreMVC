using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAreaMaster
    {
        Task<IEnumerable<DTOAreaMaster>> GetAreaById(int cityCode);
    }
}
