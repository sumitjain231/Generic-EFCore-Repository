using System.Linq.Expressions;

namespace GenericEFRepositoryFramework.Interface
{
    public interface  IGenericEFRepository<T> : IDisposable where T : class
    {
        //add the object to repository
        void Add(T entity);

        //Attach any modified entity to repository
        void Attach(T entity);

        void Delete(T entity);

        //Save changes for repository
        int SaveChanges();

        //Search from DB
        IQueryable<T> Fetch(bool isTrack = true);

        //return all object for type
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate, bool isTrack = true
            , IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null);

        T FirstOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);

        T SingleOrDefault(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);

        //Implementation for single object save  
        void Save(T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true);

        /// <summary>
        /// Implementation for collection of objects
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>
        void Save(List<T>? newEntity = null, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true);

        Task<int> SaveChangesAsync();

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool isTrack = true
           , IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);

        IEnumerable<T> ExecuteSqlQuery(string query, object[] parameter);

        Task<IEnumerable<T>> ExecuteSqlQueryAsync(string query, object[] parameter);

        int ExecuteScalerSqlQuery(string query, object[] parameter);

        Task<int> ExecuteScalerSqlQueryAsync(string query, object[] parameter);

        Task<int> SaveAsync(T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true);

        Task<int> SaveAsync(List<T>? newEntity = null, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true);

    }
}
