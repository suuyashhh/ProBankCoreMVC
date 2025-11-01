using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IKycAddressMaster
    {
        Task<IEnumerable<DTOKycAddressMaster>> GetAllKycAddress();
    }
}
