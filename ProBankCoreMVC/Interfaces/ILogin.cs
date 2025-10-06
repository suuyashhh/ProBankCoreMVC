using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ILogin
    {
        Task<bool> ValidateUserAsync(string ini, string code);
        Task SetUserJtiAsync(string userId, string jti);
        Task<string?> GetUserJtiAsync(string userId);
    }
}
