namespace ProBankCoreMVC.Interfaces
{
    public interface ILogin
    {
        Task<bool> ValidateUserAsync(string ini, string code);
    }
}
