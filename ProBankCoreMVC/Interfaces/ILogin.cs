using Models;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ILogin
    {
        Task<DTOLogin?> LoginAsync(DTOLogin login);
        Task SetUserJtiAsync(string userId, string? jti);
        Task<string?> GetUserJtiAsync(string userId);
    }
}
