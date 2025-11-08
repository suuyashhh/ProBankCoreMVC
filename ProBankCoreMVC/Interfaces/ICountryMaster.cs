using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICountryMaster
    {
        Task<DTOCountryMaster> GetCountryById(int countryCode);
        Task Save(DTOCountryMaster country_list);
    }
}
