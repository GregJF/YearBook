using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace YearBook.EntityCore.Interfaces
{
    public interface IAppointmentDBWrap<TEntity>
       where TEntity : class, IEntity
    {
        Task<TEntity> Add(TEntity entity, bool save = false);
        Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities, bool save = false);
        Task<int?> Remove(TEntity entity, bool save = false);
        Task<int?> RemoveRange(IEnumerable<TEntity> entities, bool save = false);
        Task<TEntity> Update(TEntity entity, bool save = false);
        Task<IEnumerable<TEntity>> UpdateRange(IEnumerable<TEntity> entities, bool save = false);

        Task<bool> Any(Expression<Func<TEntity, bool>> whereEpr);
        Task<int> Count(Expression<Func<TEntity, bool>> whereEpr);
        Task<List<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> whereEpr);
        Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> whereEpr);

        Task<IDbContextTransaction> BeginTransaction();
        Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel);
        Task<int> Save();
        Task Commit();
        Task Rollback();
    }
}
