using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICountryMaster
    {
        Task<IEnumerable<DTOCountryMaster>> GetAll();
        Task<DTOCountryMaster?> GetCountryById(long countryCode);
        Task Save(DTOCountryMaster country);
        Task Update(DTOCountryMaster country);
        Task Delete(long countryCode);
    }
}
