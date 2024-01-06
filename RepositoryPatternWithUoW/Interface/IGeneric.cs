using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static RepositoryPatternWithUoW.Core.Interface.IGeneric;

namespace RepositoryPatternWithUoW.Core.Interface
{
    public interface IGeneric
    {
        public interface IGeneric<T> where T : class
        {
            IQueryable<T> Get(
                Expression<Func<T, bool>> Filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null,
                string Including = null);
            Task<T> GetById(long Id);
            Task<T> GetObj(Expression<Func<T, bool>> Filter = null, string Including = null);
            Task<bool> IsExist(Expression<Func<T, bool>> Filter);
            Task<bool> Add(T Obj);
            Task<bool> Update(T Obj);
            Task<bool> Delete(T Obj);
            Task<bool> RemoveRange(T Obj);
            Task SaveChanges();

        }
    }
}
