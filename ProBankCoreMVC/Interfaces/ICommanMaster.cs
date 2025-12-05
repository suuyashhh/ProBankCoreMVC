using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICommanMaster
    {
        Task Save(DTOCommanMaster objComman);
        Task<DTOCommanMaster> GetCommanMasterById(string tableName, int code);
        Task UpdateCommanMaster(string tableName, int code, string name);
        Task DeleteCommanMaster(string tableName, int code);
        Task<IEnumerable<DTOCommanMaster>> GetAllMaster(string tableName);


    }
}
