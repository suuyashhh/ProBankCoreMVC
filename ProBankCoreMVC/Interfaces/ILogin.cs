using Models;
using System.Threading.Tasks;

namespace ProBankCoreMVC.Interfaces
{
    public interface ILogin
    {
        Task<DTOLogin> Login(DTOLogin login);
    }
}
