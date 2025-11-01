using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IDireMast
    {
        Task<IEnumerable<DTODireMast>> GetAllOther();
    }
}
