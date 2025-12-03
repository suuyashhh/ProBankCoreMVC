using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IPrefixMaster
    {
        Task<IEnumerable<DTOPrefixMaster>> GetAllPrefix();
        Task<DTOPrefixMaster> GetPrefixById(int code);
        Task Save(DTOPrefixMaster Prefix);
        Task Update(DTOPrefixMaster Prefix);
        Task Delete(long Code);

    }
}
