using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IThreeFieldMaster
    {
        Task<IEnumerable<DTOThreeFieldMaster>> GetAllThreeFieldMaster(string tableName);
        Task<DTOThreeFieldMaster> GetThreeFieldMasterById(string tableName, int code);
        Task Save(DTOThreeFieldMaster obj);
        Task<DTOThreeFieldMaster> Update(DTOThreeFieldMaster dto);
        Task<DTOThreeFieldMaster> Delete(string tableName, string code);
    }
}
