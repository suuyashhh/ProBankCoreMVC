using Microsoft.AspNetCore.Mvc;
using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IReligionMaster
    {
        Task<IEnumerable<DTOReligionMaster>> GetAllReligion();

        Task<DTOReligionMaster> GetReligionById(int code);
        Task Save(DTOReligionMaster Religion);
        Task Update(DTOReligionMaster Religion);
        Task Delete(long code);

    }
}
