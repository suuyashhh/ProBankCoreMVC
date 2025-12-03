using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICityMaster
    {
        Task<IEnumerable<DTOCityMaster>> GetAllCity();
        Task<DTOCityMaster> GetCityById(int country, int state, int dist, int taluka, int code);
        Task Save(DTOCityMaster objList);
        Task Update(DTOCityMaster objList);
        Task<DTOCityMaster> Delete(int country, int state, int dist, int taluka, int code);
        Task<IEnumerable<DTOCityMaster>> GetAllDependencies();
        Task<IEnumerable<DTODistrictMaster>> GetDistrictsByState(int countryCode, int stateCode);
        Task<IEnumerable<DTOTalukaMaster>> GetTalukasByDistrict(int countryCode, int stateCode, int distCode);
    }
}
