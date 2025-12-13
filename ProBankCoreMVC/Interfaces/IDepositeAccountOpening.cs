using Models;

namespace ProBankCoreMVC.Interfaces
{
    public interface IDepositeAccountOpening
    {
        Task<IEnumerable<DTODepositeAccountOpening>> GetGlCodeAll();
    }
}
