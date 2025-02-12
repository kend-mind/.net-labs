using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.EFCore.Implements
{
    public class ExecuteBaseRepository<TContext> : IExecuteBaseRepository<TContext> where TContext : DbContext
    {
        private DbContext Context { get; }

        public ExecuteBaseRepository(DbContext context)
        {
            Context = context;
        }

        public virtual int ExecuteNonQuery(string sql, params object[] parameters) => Context.Database.ExecuteSqlRaw(sql, parameters);
        public virtual async Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters) => await Context.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public class ExecuteableRepository<TContext, TEntityExecute> : ExecuteBaseRepository<TContext>, IExecuteableRepository<TContext>
        where TContext : DbContext
    {
        public ExecuteableRepository(TContext context) : base(context)
        {
        }
    }

    public abstract class ExecuteRepository<TContext> : IExecuteRepository<TContext> where TContext : DbContext
    {
        protected readonly IExecuteableRepository<TContext> ExecuteableRepository;

        public ExecuteRepository(IExecuteableRepository<TContext> executeableRepository)
        {
            ExecuteableRepository = executeableRepository;
        }

        public virtual int ExecuteNonQuery(string sql, params object[] parameters) => ExecuteableRepository.ExecuteNonQuery(sql, parameters);
        public virtual async Task<int> ExecuteNonQueryAsync(string sql, params object[] parameters) => await ExecuteableRepository.ExecuteNonQueryAsync(sql, parameters);
    }
}
