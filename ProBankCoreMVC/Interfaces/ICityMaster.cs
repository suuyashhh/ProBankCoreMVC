using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface ICityMaster
    {
        Task<IEnumerable<DTOCityMaster>> GetAllCity();
        //Task<IEnumerable<DTOCityMaster>> save(int CountryCode, int StateCode, int DistCode, int TalukaCode, int Code, string Name, int PinCode, string Entry_Date);
        Task<IEnumerable<DTOCityMaster>> GetAllDependencies(); 
    }
}
