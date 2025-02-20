using GenericEFRepositoryFramework.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericEFRepositoryFramework.Repository
{
    public class GenericSelectDataRepository<T, S> : IGenericSelectRepository<T, S>, IDisposable where T : class
         where S : class
    {
        private DbContext context;
        private DbSet<T> objectSet;
        private bool disposeContext = false;

        #region constructor

        public GenericSelectDataRepository() //: this(new BaseDBContainer())
        {
            context = null!;
            objectSet = null!;
        }

        public GenericSelectDataRepository(DbContext dbContext, bool disposeContext = true)
        {
            this.context = dbContext;
            this.objectSet = this.context.Set<T>();
            this.disposeContext = disposeContext;
        }

        #endregion

        #region simple Crud         

        /// <summary>
        /// Find with order by , paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<S> Find(Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>? predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> data = isTrack ? this.objectSet.AsQueryable<T>() : this.objectSet.AsNoTracking<T>();
            if (predicate != null)
            {
                data = data.Where(predicate);
            }
            //handle order by
            if (orderBy != null)
            {
                bool isFirstSort = true;
                orderBy.ToList().ForEach(one =>
                {
                    data = one.ApplySort(data, isFirstSort);
                    isFirstSort = false;
                });
            }

            // handle paging
            if (skip > 0)
            {
                data = data.Skip(skip);
            }
            if (top > 0)
            {
                data = data.Take(top);
            }
            if (includes != null)
            {
                data = includes.Aggregate(data, (current, include) => current.Include(include));
            }
            IQueryable<S> selectData = data.Select(selectPredicate);
            //return one by one object
            foreach (var item in selectData)
            {
                yield return item;
            }
        }


        public async Task<IEnumerable<S>> FindAsync(Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>? predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
        {
            IQueryable<T> data = isTrack ? this.objectSet.AsQueryable<T>() : this.objectSet.AsNoTracking<T>();
            if (predicate != null)
            {
                data = data.Where(predicate);
            }
            //handle order by
            if (orderBy != null)
            {
                bool isFirstSort = true;
                orderBy.ToList().ForEach(one =>
                {
                    data = one.ApplySort(data, isFirstSort);
                    isFirstSort = false;
                });
            }

            // handle paging
            if (skip > 0)
            {
                data = data.Skip(skip);
            }
            if (top > 0)
            {
                data = data.Take(top);
            }
            if (includes != null)
            {
                data = includes.Aggregate(data, (current, include) => current.Include(include));
            }
            IQueryable<S> selectData = data.Select(selectPredicate);
            return await selectData.ToListAsync();
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (this.context != null))
            {
                if (disposeContext)
                {
                    this.context.Dispose();
                    this.context = null!;
                }
            }
        }

        #endregion

    }
}
