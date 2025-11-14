using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IDistrictMaster
    {
        Task<DTODistrictMaster> GetDistrictById(int distCode, int Country_Code, int State_Code);
        Task<IEnumerable<DTODistrictMaster>> GetAll();
        Task Save(DTODistrictMaster district);
        Task Update(DTODistrictMaster district);
        Task Delete(long Code, int State_Code, int Country_Code);
    }
}

