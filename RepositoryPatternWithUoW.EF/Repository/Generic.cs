using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUoW.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static RepositoryPatternWithUoW.Core.Interface.IGeneric;

namespace RepositoryPatternWithUoW.EF.Repository
{
    public class Generic<T> : IGeneric<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        internal DbSet<T> _entity;
        public Generic(ApplicationDbContext context)
        {
            _context = context;
            this._entity = context.Set<T>();
        }
        public async Task<bool> Add(T Obj)
        {
            await this._entity.AddAsync(Obj);
            return await Save();
        }

        public async Task<bool> Delete(T Obj)
        {
            this._entity.Remove(Obj);
            return await Save();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> Filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null, string Including = null)
        {
            IQueryable<T> query = _entity;
            if (Filter != null)
            {
                query = query.Where(Filter);
            }
            if (Including != null)
            {
                foreach (var item in Including.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            if (OrderBy != null)
            {
                return OrderBy(query);
            }
            return query;
        }

        public async Task<T> GetById(long Id)
        {
            return await _entity.FindAsync(Id);
        }
        public async Task<bool> IsExist(System.Linq.Expressions.Expression<Func<T, bool>> Filter)
        {
            return await _entity.AnyAsync(Filter);
        }
        public async Task<T> GetObj(Expression<Func<T, bool>> Filter = null, string Including = null)
        {
            IQueryable<T> query = _entity;
            if (Filter != null)
            {
                query = query.Where(Filter);
            }
            if (Including != null)
            {
                foreach (var item in Including.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<bool> RemoveRange(T Obj)
        {
            this._entity.RemoveRange(Obj);
            return await Save();
        }
        public async Task<bool> Update(T Obj)
        {
            _context.Entry<T>(Obj).State = EntityState.Modified;
            return await Save();
        }
        public async Task SaveChanges()
        {
           await _context.SaveChangesAsync();
        }
        private async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        ~Generic()
        {
            _context.Dispose();
        }
    }
}
