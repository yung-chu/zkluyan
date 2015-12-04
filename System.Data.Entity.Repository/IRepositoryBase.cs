using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Common;

namespace System.Data.Entity.Repository
{
    public interface IRepositoryBase : IDisposable
    {

        /// <summary>
        /// Add an entity to the context.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add<R>(R entity) where R : class;

        /// <summary>
        /// Add multiple entities to the context.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddMultiple<R>(List<R> entities) where R : class;

        /// <summary>
        /// Add multiple entities to the context and commit to database.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddMultipleWithCommit<R>(List<R> entities) where R : class;

        /// <summary>
        /// Add an entity to the context or update the entity if it already exists.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool AddOrUpdate<R>(R entity) where R : class;

        /// <summary>
        /// Add multiple entities to the context or update them if they already exsist.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddOrUpdateMultiple<R>(List<R> entities) where R : class;

        /// <summary>
        /// Add multiple entities to the context or update them if they already exsist and commit to database.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool AddOrUpdateMultipleCommit<R>(List<R> entities) where R : class;

        /// <summary>
        /// Commit the transaction to the database.
        /// </summary>
        /// <param name="startNewTransaction"></param>
        void CommitTransaction(bool startNewTransaction = false);

        /// <summary>
        /// Count all entities of a specific type.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <returns></returns>
        Int32 Count<R>() where R : class;

        void Dispose();

        bool Delete<R>(R entity) where R : class;
        bool DeleteMultiple<R>(List<R> entities) where R : class;
        bool DeleteMultipleWithCommit<R>(List<R> entities) where R : class;

        void Detach(object entity);
        void Detach(List<object> entities);

        IQueryable<R> Find<R>(Expression<Func<R, bool>> where) where R : class;
        IQueryable<R> Find<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class;

        R First<R>(Expression<Func<R, bool>> where) where R : class;
        R FirstOrDefault<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class;

        IQueryable<R> GetAll<R>() where R : class;
        IQueryable<R> GetAll<R>(params Expression<Func<R, object>>[] includes) where R : class;

        DbConnection GetConnection();

        void SaveChanges();
        void SetIdentityCommand();
        void SetConnectionString(string connectionString);
        void SetIsolationLevel(IsolationLevel isolationLevel);
        void SetRethrowExceptions(bool rehtrowExceptions);
        void SetTransactionType(TransactionTypes transactionType);
        void SetUseTransaction(bool useTransaction);

        R Single<R>(Expression<Func<R, bool>> where) where R : class;
        R SingleOrDefault<R>(Expression<Func<R, bool>> where) where R : class;
        R SingleOrDefault<R>(Expression<Func<R, bool>> where, Expression<Func<R, object>> include) where R : class;

        bool Update<R>(R entity) where R : class;
        bool UpdateProperty<R>(R entity, params Expression<Func<R, object>>[] properties) where R : class;
        bool UpdateMultiple<R>(List<R> entities) where R : class;
        bool UpdateMultipleWithCommit<R>(List<R> entities) where R : class;
    }
}
