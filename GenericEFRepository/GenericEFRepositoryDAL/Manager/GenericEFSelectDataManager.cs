using EFDomainData.Constant;
using GenericEFRepositoryFramework.Interface;
using GenericEFRepositoryFramework.Manager;
using GenericEFRepositoryFramework.Repository;
using System.Linq.Expressions;

namespace GenericEFRepositoryDAL.Manager
{
    public class GenericEFSelectDataManager<T, S> : RepositoryManager<GenericEFSelectDataManager<T, S>> where T : class where S : class
    {
        /// <summary>
        /// find data asynchrony
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<S>> FindAsync(string databaseName, Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>? predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int take = 0, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericSelectDataRepository<T, S> repoType = new GenericSelectDataRepository<T, S>(Databases.GetDatabaseContext(databaseName)))
            {
                return await repoType.FindAsync(selectPredicate, predicate, isTrack, orderBy, skip, take, includes);
            }
        }

        /// <summary>
        /// find data sync
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="isTrack"></param>
        /// <param name="orderBy"></param>
        /// <param name="skip"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static IList<S> Find(string databaseName, Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>? predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericSelectDataRepository<T, S> repoType = new GenericSelectDataRepository<T, S>(Databases.GetDatabaseContext(databaseName)))
            {
                return repoType.Find(selectPredicate, predicate, isTrack, orderBy, skip, top, includes).ToList();
            }
        }
    }
}
