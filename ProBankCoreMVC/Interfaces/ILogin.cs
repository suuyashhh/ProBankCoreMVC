namespace ProBankCoreMVC.Interfaces
{
    public interface ILogin
    {
        Task<bool> ValidateUserAsync(string mobileNo, string code);
    }
}
