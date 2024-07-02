using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YearBook.EntityCore.Interfaces;

namespace YearBook.EntityCore
{
    internal abstract class AppointmentDbWrap<TEntity> : IAppointmentDBWrap<TEntity>
       where TEntity : class, IEntity
    {
        protected readonly AppointmentContext context;
        public AppointmentDbWrap(IAppointmentDBContext context)
        {
            this.context = context as AppointmentContext;
        }

        public async Task<TEntity> Add(TEntity entity, bool save = false)
        {
            await context.Set<TEntity>().AddAsync(entity);
            if (save)
            {
                await Save();
            }
            return entity;
        }

        public async Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities, bool save = false)
        {
            await context.AddRangeAsync(entities);
            if (save)
            {
                await Save();
            }
            return entities;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> whereEpr)
        {
            return await context.Set<TEntity>().AnyAsync(whereEpr);
        }


        public async Task<int> Count(Expression<Func<TEntity, bool>> whereEpr)
        {
            return await context.Set<TEntity>().CountAsync(whereEpr);
        }

        public async Task<List<TEntity>> FindByCondition(Expression<Func<TEntity, bool>> whereEpr)
        {
            return await context.Set<TEntity>().Where(whereEpr).ToListAsync();
        }

        public async Task<TEntity> FindFirstByCondition(Expression<Func<TEntity, bool>> whereEpr)
        {
            return await context.Set<TEntity>().Where(whereEpr).FirstOrDefaultAsync();
        }

        public async Task<int?> Remove(TEntity entity, bool save = false)
        {
            context.Set<TEntity>().Remove(entity);
            if (save)
            {
                return await Save();
            }
            return null;
        }

        public async Task<int?> RemoveRange(IEnumerable<TEntity> entities, bool save = false)
        {
            context.RemoveRange(entities);
            if (save)
            {
                return await Save();
            }
            return null;
        }

        public async Task<TEntity> Update(TEntity entity, bool save = false)
        {
            context.Set<TEntity>().Update(entity);
            if (save)
            {
                await Save();
            }
            return entity;
        }

        public async Task<IEnumerable<TEntity>> UpdateRange(IEnumerable<TEntity> entities, bool save = false)
        {
            context.UpdateRange(entities);
            if (save)
            {
                await Save();
            }
            return entities;
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            return await context.Database.BeginTransactionAsync();
        }

        public async Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel)
        {
            return await context.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }

        public async Task Commit()
        {
            await context.Database.CommitTransactionAsync();
        }
        public async Task Rollback()
        {
            await context.Database.RollbackTransactionAsync();
        }

    }
}
