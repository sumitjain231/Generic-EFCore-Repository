using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GenericEFRepositoryFramework.Interface
{
    public interface IGenericSelectRepository<T, S> : IDisposable
        where T : class
        where S : class
    {
        //return all object for type
        IEnumerable<S> Find(Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>? predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null);

        Task<IEnumerable<S>> FindAsync(Expression<Func<T, S>> selectPredicate, Expression<Func<T, bool>>?predicate = null
            , bool isTrack = true, IOrderByClause<T>[]? orderBy = null, int skip = 0, int top = 0, Expression<Func<T, object>>[]? includes = null);

    }
}
