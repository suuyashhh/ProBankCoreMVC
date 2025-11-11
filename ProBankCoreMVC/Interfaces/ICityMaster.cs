using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICityMaster
    {
        Task<IEnumerable<DTOCityMaster>> GetAllDependencies(); 
    }
}
