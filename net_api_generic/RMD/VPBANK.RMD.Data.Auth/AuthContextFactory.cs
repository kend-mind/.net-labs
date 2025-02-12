using Microsoft.EntityFrameworkCore.Design;

namespace VPBANK.RMD.Data.Auth
{
    public class AuthContextFactory : IDesignTimeDbContextFactory<AuthContext>
    {
        public AuthContext CreateDbContext(string[] args)
        {
            //var optionsBuilder = new DbContextOptionsBuilder<AuthContext>();
            //optionsBuilder.UseSqlServer("Server=10.37.16.226\\dev; Database=Auth; User ID=dev_user; Password=12345a@");
            //return new AuthContext(optionsBuilder.Options);
            return null;
        }
    }
}
