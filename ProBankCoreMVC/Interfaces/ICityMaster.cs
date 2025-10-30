using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICityMaster
    {
        Task<IEnumerable<DTOCityMaster>> GetAllCities();
        Task<DTOCityMaster> GetDependencyByCityId(int cityUnicId);
    }
}
