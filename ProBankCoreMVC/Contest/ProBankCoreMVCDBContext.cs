using Microsoft.EntityFrameworkCore;
using Models;

namespace ProBankCoreMVC.Contest
{
    public class ProBankCoreMVCDBContext:DbContext
    {
        public ProBankCoreMVCDBContext(DbContextOptions<ProBankCoreMVCDBContext> options) : base(options)
        {
        }
        public DbSet<Login> Login { get; set; }
    }
}
