using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICastMaster
    {
        Task<IEnumerable<DTOCastMaster>> GetAllCast();
    }
}
