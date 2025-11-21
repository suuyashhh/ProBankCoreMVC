// IValidationService.cs
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface IValidationService
    {
        Task<bool> AadharNo(string aadharNo);
        Task<bool>PanNo(string panNo);
        Task<bool> GstNo(string gstNo);
        Task<bool> MobileNo(string mobileNo);
        Task<bool> PhoneNo(string phone1);
        Task<bool> VoterIdNo(string voterIdNo);
        Task<bool> PassportNo(string passportNo);
    }
}
