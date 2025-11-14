// IValidationService.cs
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface IValidationService
    {
        Task<bool> AadharNo(string aadharNo);
    }
}
