using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICityMaster
    {
        Task<IEnumerable<DTOCityMaster>> GetAllCity();
        Task<DTOCityMaster> GetCityById(DTOCityMaster objList);
        Task Save(DTOCityMaster objList);
        Task Update(DTOCityMaster objList);
        Task<bool> Delete(DTOCityMaster objList);
        Task<IEnumerable<DTOCityMaster>> GetAllDependencies();
    }
}
