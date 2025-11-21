using Models;
using static Models.DTOPartyMaster;

namespace ProBankCoreMVC.Interfaces
{
    public interface IPartyMaster
    {
        Task<List<DTOPartyMaster.CustomerSummary>> GetCustomers(int branchCode, string? search = null);
        Task<DTOPartyMaster> GetCustomerById(int Cust_Code);
        Task<string> Save(DTOPartyMaster p);
    }
}
