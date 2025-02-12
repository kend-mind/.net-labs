using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VPBANK.RMD.EFCore.Abstractions
{
    public interface IExecuteBaseRepository<TContext> where TContext : DbContext
    {
        int ExecuteNonQuery(string sql, params object[] parameters);
        Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters);
    }

    public interface IExecuteableRepository<TContext> : IExecuteBaseRepository<TContext> where TContext : DbContext
    {
    }

    public interface IExecuteRepository<TContext> : IExecuteableRepository<TContext> where TContext : DbContext
    {
    }
}
