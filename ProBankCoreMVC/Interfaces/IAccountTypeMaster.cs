using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAccountTypeMaster
    {
        Task<IEnumerable<DTOAccountTypeMaster>> GetAllAccountType();
        Task<DTOAccountTypeMaster> GetAccountTypeById(int Code);
        Task Save(DTOAccountTypeMaster AccountType);
        Task Update(DTOAccountTypeMaster AccountType);
        Task Delete(long Code);
    }
}
