using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IKycIdMaster
    {
        Task<IEnumerable<DTOKycIdMaster>> GetAllKycId();
    }
}
