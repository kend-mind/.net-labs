using Microsoft.EntityFrameworkCore.Design;

namespace VPBANK.RMD.Data.IFRS9_Data
{
    public class IFRS9_DataContextFactory : IDesignTimeDbContextFactory<IFRS9_DataContext>
    {
        public IFRS9_DataContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<PhoenixConfContext>();
            //optionsBuilder.UseSqlServer("Server=10.37.16.226\\dev; Database=IFRS9_Data; User ID=dev_user; Password=12345a@");
            //return new PhoenixConfContext(optionsBuilder.Options);
            return null;
        }
    }
}
