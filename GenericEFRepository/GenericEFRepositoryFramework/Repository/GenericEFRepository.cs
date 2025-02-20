using GenericEFRepositoryFramework.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericEFRepositoryFramework.Repository
{
    public class GenericEFRepository<T> : IGenericEFRepository<T>, IDisposable where T : class
    {
        private DbContext context;
        private DbSet<T> objectSet;
        private bool disposeContext = false;

        #region constructor

        public GenericEFRepository() //: this(new BaseDBContainer())
        {
            context = null!;
            objectSet = null!;
        }

        public GenericEFRepository(DbContext dbContext, bool disposeContext = true)
        {
            this.context = dbContext;
            this.objectSet = this.context.Set<T>();
            this.disposeContext = disposeContext;
        }

        #endregion

        #region simple Crud

        public void Add(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Added;
        }

        public void Attach(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();

        }
        public IQueryable<T> Fetch(bool isTrack = true)
        {

            return isTrack ? this.objectSet.AsQueryable<T>() : this.objectSet.AsNoTracking<T>();
        }

        /// <summary>
        /// Find with order by , paging
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, bool isTrack = true
              , IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
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
            //return one by one object
            foreach (var item in data)
            {
                yield return item;
            }
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            if (includes != null)
            {
                var result = includes.Aggregate(this.objectSet.AsQueryable(), (current, include) => current.Include(include));
                return result.FirstOrDefault<T>(predicate)!;
            }
            else
                return this.objectSet.FirstOrDefault<T>(predicate)!;
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            if (includes != null)
            {
                var result = includes.Aggregate(this.objectSet.AsQueryable(), (current, include) => current.Include(include));
                return result.SingleOrDefault<T>(predicate)!;
            }
            else
                return this.objectSet.SingleOrDefault<T>(predicate)!;
        }

        public IEnumerable<T> ExecuteSqlQuery(string query, params object[] parameter)
        {
            return this.objectSet.FromSqlRaw<T>(query, parameter);
        }

        public int ExecuteScalerSqlQuery(string query, params object[] parameter)
        {
            return this.context.Database.ExecuteSqlRaw(query, parameter);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();

        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTrack = true
             , IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
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
            return await data.ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            if (includes != null)
            {
                var result = includes.Aggregate(this.objectSet.AsQueryable(), (current, include) => current.Include(include));
                return (await result.FirstOrDefaultAsync<T>(predicate))!;
            }
            else
                return (await this.objectSet.FirstOrDefaultAsync<T>(predicate))!;
        }


        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            if (includes != null)
            {
                var result = includes.Aggregate(this.objectSet.AsQueryable(), (current, include) => current.Include(include));
                return (await result.SingleOrDefaultAsync<T>(predicate))!;
            }
            else
                return (await this.objectSet.SingleOrDefaultAsync<T>(predicate))!;
        }
        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync(string query, params object[] parameter)
        {
            return await this.objectSet.FromSqlRaw<T>(query, parameter).ToListAsync();
        }

        public async Task<int> ExecuteScalerSqlQueryAsync(string query, params object[] parameter)
        {
            return await this.context.Database.ExecuteSqlRawAsync(query, parameter);
        }

        #endregion

        #region Aggregate Functions

        /// <summary>
        /// Get max data row for orderby Id
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="isTrack"></param>
        /// <returns></returns>
        public async Task<T> MaxAsync(IOrderByClause<T> orderBy, bool isTrack = true)
        {
            IQueryable<T> data = isTrack ? this.objectSet.AsQueryable<T>() : this.objectSet.AsNoTracking<T>();
            bool isFirstSort = true;
            data = orderBy.ApplySort(data, isFirstSort);
            return (await data.FirstOrDefaultAsync())!;
        }

        #endregion

        #region complex Save

        /// <summary>
        /// Save on single object
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>

        public void Save(T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true)
        {
            if (newEntity == null && modifiedEntity == null && deletedEntity == null)
            {
                throw new ArgumentNullException("newEntity && modifiedEntity && deletedEntity is null");
            }

            if (newEntity != null)
            {
                context.Entry<T>(newEntity).State = EntityState.Added;
            }

            if (modifiedEntity != null)
            {
                context.Entry<T>(modifiedEntity).State = EntityState.Modified;
            }
            if (deletedEntity != null)
            {
                context.Entry<T>(deletedEntity).State = EntityState.Deleted;
            }
            if (commitChanges)
            {
                this.SaveChanges();
            }
        }

        /// <summary>
        /// Save for collection
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>

        public void Save(List<T>? newEntity, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true)
        {
            if (newEntity == null && modifiedEntity == null && deletedEntity == null)
            {
                throw new ArgumentNullException("newEntity && modifiedEntity  && deletedEntity collection is null");
            }
            if (newEntity != null && newEntity.Count > 0)
            {
                foreach (var obj in newEntity)
                    context.Entry<T>(obj).State = EntityState.Added;
            }
            if (modifiedEntity != null && modifiedEntity.Count > 0)
            {
                foreach (var obj in modifiedEntity)
                    context.Entry<T>(obj).State = EntityState.Modified;
            }
            if (deletedEntity != null && deletedEntity.Count > 0)
            {
                foreach (var obj in deletedEntity)
                    context.Entry<T>(obj).State = EntityState.Deleted;
            }
            if (commitChanges)
            {
                this.SaveChanges();
            }
        }


        /// <summary>
        /// Save on single object
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>

        public async Task<int> SaveAsync(T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true)
        {
            if (newEntity == null && modifiedEntity == null && deletedEntity == null)
            {
                throw new ArgumentNullException("newEntity && modifiedEntity && deletedEntity is null");
            }

            if (newEntity != null)
            {
                context.Entry<T>(newEntity).State = EntityState.Added;
            }

            if (modifiedEntity != null)
            {
                context.Entry<T>(modifiedEntity).State = EntityState.Modified;
            }
            if (deletedEntity != null)
            {
                context.Entry<T>(deletedEntity).State = EntityState.Deleted;
            }
            if (commitChanges)
            {
                return await this.SaveChangesAsync();
            }
            return 0;
        }

        /// <summary>
        /// Save for collection
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>

        public async Task<int> SaveAsync(List<T>? newEntity, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true)
        {
            if (newEntity == null && modifiedEntity == null && deletedEntity == null)
            {
                throw new ArgumentNullException("newEntity && modifiedEntity && deletedEntity collection is null");
            }
            if (newEntity != null)
            {
                foreach (var obj in newEntity)
                    context.Entry<T>(obj).State = EntityState.Added;
            }
            if (modifiedEntity != null)
            {
                foreach (var obj in modifiedEntity)
                    context.Entry<T>(obj).State = EntityState.Modified;
            }
            if (deletedEntity != null)
            {
                foreach (var obj in deletedEntity)
                    context.Entry<T>(obj).State = EntityState.Deleted;
            }
            if (commitChanges)
            {
                return await this.SaveChangesAsync();
            }
            return 0;
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
