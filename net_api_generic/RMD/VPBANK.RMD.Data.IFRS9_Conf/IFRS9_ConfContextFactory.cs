using Microsoft.EntityFrameworkCore.Design;

namespace VPBANK.RMD.Data.IFRS9_Conf
{
    public class IFRS9_ConfContextFactory : IDesignTimeDbContextFactory<IFRS9_ConfContext>
    {
        public IFRS9_ConfContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<PhoenixConfContext>();
            //optionsBuilder.UseSqlServer("Server=10.37.16.226\\dev; Database=IFRS9_Conf; User ID=dev_user; Password=12345a@");
            //return new PhoenixConfContext(optionsBuilder.Options);
            return null;
        }
    }
}
