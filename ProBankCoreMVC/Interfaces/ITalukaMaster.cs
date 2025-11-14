using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ITalukaMaster
    {
        Task<DTOTalukaMaster> GetTalukaById(int talukaCode);
        Task<IEnumerable<DTOTalukaMaster>> GetTaluka(int Dist_code, int State_Code, int Country_Code);
        Task<IEnumerable<DTOTalukaMaster>> GetAll();
        Task Save(DTOTalukaMaster taluka);
        Task Update(DTOTalukaMaster taluka);
        Task Delete(long Code, int Dist_code, int State_Code, int Country_Code);

    }
}
