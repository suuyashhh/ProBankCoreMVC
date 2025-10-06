using System;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface IAuthService
    {
        Task<(string token, DateTime expires)> AuthenticateAndCreateTokenAsync(string ini, string code);
    }
}
