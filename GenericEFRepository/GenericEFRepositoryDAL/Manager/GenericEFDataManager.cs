using EFDomainData.Constant;
using GenericEFRepositoryFramework.Interface;
using GenericEFRepositoryFramework.Manager;
using GenericEFRepositoryFramework.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GenericEFRepositoryDAL.Manager
{
    public class GenericEFDataManager<T> : RepositoryManager<GenericEFDataManager<T>> where T : class
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
        public static async Task<IEnumerable<T>> FindAsync(string databaseName, Expression<Func<T, bool>> predicate, bool isTrack = true
             , IOrderByClause<T>[]? orderBy = null, int skip = 0, int take = 0, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return await repoType.FindAsync(predicate, isTrack, orderBy, skip, take, includes);
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
        public static IList<T> Find(string databaseName, Expression<Func<T, bool>> predicate, bool isTrack = true
            , IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return repoType.Find(predicate, isTrack, orderBy, skip, top, includes).ToList();
            }
        }

        /// <summary>
        /// get first record asyncsly
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsync(string databaseName, Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return await repoType.FirstOrDefaultAsync(predicate, includes);
            }
        }

        /// <summary>
        /// get first record
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T FirstOrDefault(string databaseName, Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return repoType.FirstOrDefault(predicate, includes);
            }
        }

        /// <summary>
        /// get single record
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static async Task<T> SingleOrDefaultAsync(string databaseName, Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return await repoType.SingleOrDefaultAsync(predicate, includes);
            }
        }

        /// <summary>
        /// get single record
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static T SingleOrDefault(string databaseName, Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return repoType.SingleOrDefault(predicate, includes);
            }
        }

        /// <summary>
        /// execute sql query async
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<T>> ExecuteSqlQueryAsync(string databaseName, string query, object[] parameter, int connectionTimeout = 0)
        {
            var dbContext = Databases.GetDatabaseContext(databaseName);
            if (connectionTimeout != 0)
            {
                dbContext.Database.SetCommandTimeout(connectionTimeout);
            }
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(dbContext))
            {
                return await repoType.ExecuteSqlQueryAsync(query, parameter);
            }

        }

        /// <summary>
        /// Execute sql query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static List<T> ExecuteSqlQuery(string databaseName, string query, object[] parameter, int connectionTimeout = 0)
        {
            var dbContext = Databases.GetDatabaseContext(databaseName);
            if (connectionTimeout != 0)
            {
                dbContext.Database.SetCommandTimeout(connectionTimeout);
            }
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(dbContext))
            {
                return repoType.ExecuteSqlQuery(query, parameter).ToList();
            }

        }

        /// <summary>
        /// execute sql query async
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteScalerSqlQueryAsync(string databaseName, string query, object[] parameter, int connectionTimeout = 0)
        {

            var dbContext = Databases.GetDatabaseContext(databaseName);
            if (connectionTimeout != 0)
            {
                dbContext.Database.SetCommandTimeout(connectionTimeout);
            }
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(dbContext))
            {
                return await repoType.ExecuteScalerSqlQueryAsync(query, parameter);
            }

        }

        /// <summary>
        /// Execute sql query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static int ExecuteScalerSqlQuery(string databaseName, string query, params object[] parameter)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return repoType.ExecuteScalerSqlQuery(query, parameter);
            }
        }

        /// <summary>
        /// Get max data row for orderby Id
        /// </summary>
        /// <param name="orderBy"></param>
        /// <param name="isTrack"></param>
        /// <returns></returns>
        public static async Task<T> MaxAsync(string databaseName, IOrderByClause<T> orderBy, bool isTrack = true)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName)))
            {
                return await repoType.MaxAsync(orderBy, isTrack);
            }
        }

        /// <summary>
        /// Save Records
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>
        /// <param name="commitChanges"></param>
        public static void Save(string databaseName, string userId, T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName, userId)))
            {
                repoType.Save(newEntity, modifiedEntity, deletedEntity, commitChanges);
            }
        }

        /// <summary>
        /// Save multiple list
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>
        /// <param name="commitChanges"></param>
        public static void Save(string databaseName, string userId, List<T> newEntity, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName, userId)))
            {
                repoType.Save(newEntity, modifiedEntity, deletedEntity, commitChanges);
            }

        }

        /// <summary>
        /// Save Sync method
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        public static async Task<int> SaveAsync(string databaseName, string userId, T? newEntity = null, T? modifiedEntity = null, T? deletedEntity = null, bool commitChanges = true)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName, userId)))
            {
                return await repoType.SaveAsync(newEntity, modifiedEntity, deletedEntity, commitChanges);
            }
        }

        /// <summary>
        /// Save Async method
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="modifiedEntity"></param>
        /// <param name="commitChanges"></param>
        /// <returns></returns>
        public static async Task<int> SaveAsync(string databaseName, string userId, List<T> newEntity, List<T>? modifiedEntity = null, List<T>? deletedEntity = null, bool commitChanges = true)
        {
            using (GenericEFRepository<T> repoType = new GenericEFRepository<T>(Databases.GetDatabaseContext(databaseName, userId)))
            {
                return await repoType.SaveAsync(newEntity, modifiedEntity, deletedEntity, commitChanges);
            }
        }
    }   
}
