using Microsoft.EntityFrameworkCore.Design;

namespace VPBANK.RMD.Data.PhoenixConf
{
    public class PhoenixConfContextFactory : IDesignTimeDbContextFactory<PhoenixConfContext>
    {
        public PhoenixConfContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<PhoenixConfContext>();
            //optionsBuilder.UseSqlServer("Server=10.37.16.226\\dev; Database=Phoenix_Conf; User ID=dev_user; Password=12345a@");
            //return new PhoenixConfContext(optionsBuilder.Options);
            return null;
        }
    }
}
