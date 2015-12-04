using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Repository
{
    public class RepositoryBase<T> : IRepositoryBase, IDisposable
        where T : DbContext
    {
        public DbContext Model;

        internal TransactionTypes _transactionType = TransactionTypes.DbTransaction;
        internal TransactionScope _transactionScope;
        internal IsolationLevel _isolationLevel = IsolationLevel.ReadUncommitted;
        internal DbTransaction _transaction;
        internal DbConnection _connection;

        internal bool _proxyCreationEnabled = false;
        internal bool _rethrowExceptions = true;
        internal bool _useTransaction = false;
        internal bool _saveLastExcecutedMethodInfo = false;

        internal int _commandTimeout = 300;

        internal string _connectionString = string.Empty;

        internal MethodBase _lastExecutedMethod = null;

        public event RepositoryBaseExceptionHandler RepositoryBaseExceptionRaised;
        public delegate void RepositoryBaseExceptionHandler(Exception exception);

        public event RepositoryBaseRollBackOccuredHandler RepositoryBaseRollBackRaised;
        public delegate void RepositoryBaseRollBackOccuredHandler(MethodBase lastExecutedMethod);

        internal static bool _staticLazyConnectionOverrideUsed = false;
        internal static bool _lazyConnection = false;

        internal void InitializeRepository()
        {
            if (Model == null)
            {
                DbContext instance = (DbContext)Activator.CreateInstance(typeof(T));
                ((IObjectContextAdapter)instance).ObjectContext.CommandTimeout = 300;
                Model = instance;

                Model.Configuration.ProxyCreationEnabled = _proxyCreationEnabled;
            }
            else
            {
                Model.Configuration.LazyLoadingEnabled = false;
            }
        }

        internal void InitializeConnection()
        {
            if (Model != null)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    Model.Database.Connection.ConnectionString = _connectionString;
                }

                if (_useTransaction)
                {
                    _connection = ((IObjectContextAdapter)Model).ObjectContext.Connection;
                    _connection.Open();
                }
            }
        }

        public RepositoryBase()
        {
            InitializeRepository();
        }

        public RepositoryBase(bool throwExceptions, string connectionString = "", bool lazyConnection = false, TransactionTypes transactionType = TransactionTypes.DbTransaction, IsolationLevel isolationLevel = IsolationLevel.Snapshot,
            bool useTransactions = true, bool proxyCreationEnabled = false, int commandTimeout = 300, bool saveLastExecutedMethodInfo = false)
        {
            _rethrowExceptions = throwExceptions;
            _useTransaction = useTransactions;
            _proxyCreationEnabled = proxyCreationEnabled;
            _commandTimeout = commandTimeout;
            _isolationLevel = IsolationLevel.Snapshot;
            _connectionString = connectionString;
            _saveLastExcecutedMethodInfo = saveLastExecutedMethodInfo;

            if (_staticLazyConnectionOverrideUsed == false)
                _lazyConnection = lazyConnection;

            InitializeRepository();
        }

        public RepositoryBase(RepositoryBaseConfiguration configuration)
        {
            _rethrowExceptions = configuration.RethrowExceptions;
            _useTransaction = configuration.UseTransaction;
            _proxyCreationEnabled = configuration.ProxyCreationEnabled;
            _commandTimeout = configuration.CommandTimeout;
            _isolationLevel = configuration.IsolationLevel;
            _connectionString = configuration.ConnectionString;
            _saveLastExcecutedMethodInfo = configuration.SaveLastExecutedMethodInfo;

            if (_staticLazyConnectionOverrideUsed == false)
                _lazyConnection = configuration.LazyConnection;

            InitializeRepository();
        }

        public bool Add<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    SetEntity<R>()
                        .Add(entity);

                    SaveChanges();

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            OnRepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }

            });

            return result;
        }

        public bool AddMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Add<R>(e));

            return result;
        }

        public bool AddMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = Add<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public bool AddOrUpdate<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        if (entry.State == EntityState.Detached)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);
                }
                finally
                { }

            });

            return result;
        }

        public bool AddOrUpdateMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = AddOrUpdate<R>(e));

            return result;
        }

        public bool AddOrUpdateMultipleCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = AddOrUpdate<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public void CommitTransaction(bool startNewTransaction = false)
        {
            if (_useTransaction)
            {
                switch (_transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (_transaction != null && _transaction.Connection != null)
                        {
                            SaveChanges();
                            _transaction.Commit();
                        }
                        break;

                    case TransactionTypes.TransactionScope:
                        try
                        {
                            if (_transactionScope != null)
                                _transactionScope.Complete();
                        }
                        catch (Exception error)
                        {
                            if (_rethrowExceptions)
                            {
                                throw;
                            }
                            else
                            {
                                if (RepositoryBaseExceptionRaised != null)
                                {
                                    RepositoryBaseExceptionRaised(error);
                                }
                            }
                        }

                        break;
                }

                if (startNewTransaction)
                    StartTransaction();
            }
            else
            {
                SaveChanges();
            }
        }

        public Int32 Count<R>() where R : class
        {
            return Model.Set<R>()
                .Count();
        }

        public bool Delete<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        entry.State = System.Data.Entity.EntityState.Deleted;
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    Model.Set<R>().Remove(entity);

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }

            });

            return result;
        }

        public bool DeleteMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Delete<R>(e));

            return result;
        }

        public bool DeleteMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = Delete<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)Model).ObjectContext;
            var entry = Model.Entry(entity);

            if (entry.State != EntityState.Detached)
                objectContext.Detach(entity);
        }

        public void Detach(List<object> entities)
        {
            entities.ForEach(e => Detach(e));
        }

        public IQueryable<R> Find<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = default(IQueryable<R>);

            ProcessTransactionableMethod(() =>
            {
                entities = SetEntities<R>().Where(where);
            });

            return entities;
        }

        public IQueryable<R> Find<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            ProcessTransactionableMethod(() =>
            {
                if (includes != null)
                {
                    entities = ApplyIncludesToQuery<R>(entities, includes);
                }

                entities = entities.Where(where);
            });

            return entities;
        }

        public R First<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .First(where);
            });

            return entity;
        }

        public R FirstOrDefault<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                if (where != null)
                    entities = entities.Where(where);

                if (includes != null)
                {
                    entities = ApplyIncludesToQuery<R>(entities, includes);
                }

                entity = entities.FirstOrDefault();
            });

            return entity;
        }

        public IQueryable<R> GetAll<R>() where R : class
        {
            IQueryable<R> entities = default(IQueryable<R>);

            ProcessTransactionableMethod(() =>
            {
                entities = SetEntities<R>();
            });

            return entities;
        }

        public IQueryable<R> GetAll<R>(params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            if (includes != null)
            {
                entities = ApplyIncludesToQuery<R>(entities, includes);
            }

            return entities;
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public void SaveChanges()
        {
            Model.SaveChanges();
        }

        public void SetIdentityCommand()
        {
            List<EntitySetBase> sets;

            var container =
                   ((IObjectContextAdapter)Model).ObjectContext.MetadataWorkspace
                      .GetEntityContainer(
                            ((IObjectContextAdapter)Model).ObjectContext.DefaultContainerName,
                            DataSpace.CSpace);

            sets = container.BaseEntitySets.ToList();

            foreach (EntitySetBase set in sets)
            {
                string command = string.Format("SET IDENTITY_INSERT {0} {1}", set.Name, "ON");
                ((IObjectContextAdapter)Model).ObjectContext.ExecuteStoreCommand(command);
            }
        }

        public void SetConnectionString(string connectionString)
        {
            if (_lazyConnection == true)
            {
                _connectionString = connectionString;
                InitializeConnection();
            }
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        public void SetRethrowExceptions(bool rehtrowExceptions)
        {
            _rethrowExceptions = rehtrowExceptions;
        }

        public void SetTransactionType(TransactionTypes transactionType)
        {
            _transactionType = transactionType;
        }

        public void SetUseTransaction(bool useTransaction)
        {
            _useTransaction = useTransaction;

            if (_lazyConnection == false)
                InitializeConnection();
        }

        public R Single<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .Single(where);
            });

            return entity;
        }

        public R SingleOrDefault<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .SingleOrDefault(where);
            });

            return entity;
        }

        public R SingleOrDefault<R>(Expression<Func<R, bool>> where, Expression<Func<R, object>> include) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .Include(include)
                    .SingleOrDefault(where);
            });

            return entity;
        }

        public bool Update<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    SaveChanges();

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }
            });

            return result;
        }

        public bool UpdateProperty<R>(R entity, params Expression<Func<R, object>>[] properties) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        Model.Set<R>().Attach(entity);

                        foreach (var property in properties)
                        {
                            MemberExpression body = property.Body as MemberExpression;

                            if (body == null)
                            {
                                UnaryExpression ubody = (UnaryExpression)property.Body;
                                body = ubody.Operand as MemberExpression;
                            }

                            entry.Property(body.Member.Name).IsModified = true;
                        }
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    SaveChanges();

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = SetEntry(entity);

                    foreach (var property in properties)
                    {
                        MemberExpression body = property.Body as MemberExpression;

                        if (body == null)
                        {
                            UnaryExpression ubody = (UnaryExpression)property.Body;
                            body = ubody.Operand as MemberExpression;
                        }

                        entry.Property(body.Member.Name).IsModified = false;
                    }

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }
            });

            return result;
        }

        public bool UpdateMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Update<R>(e));

            return result;
        }

        public bool UpdateMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = Update<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public static void UseLazyConnection(bool useLazyConnection)
        {
            _lazyConnection = useLazyConnection;
            _staticLazyConnectionOverrideUsed = true;
        }

        internal IQueryable<R> ApplyIncludesToQuery<R>(IQueryable<R> entities, Expression<Func<R, object>>[] includes) where R : class
        {
            if (includes != null)
                entities = includes.Aggregate(entities, (current, include) => current.Include(include));

            return entities;
        }

        internal void ProcessTransactionableMethod(Action action)
        {
            if (_saveLastExcecutedMethodInfo)
                _lastExecutedMethod = MethodInfo.GetCurrentMethod();

            StartTransaction();
            action();
        }

        internal IQueryable<R> SetEntities<R>() where R : class
        {
            IQueryable<R> entities = Model.Set<R>();

            return entities;
        }

        internal DbSet<R> SetEntity<R>() where R : class
        {
            DbSet<R> entity = Model.Set<R>();

            return entity;
        }

        internal DbEntityEntry SetEntry<R>(R entity) where R : class
        {
            DbEntityEntry entry = Model.Entry(entity);

            return entry;
        }

        internal IQueryable<T> GetQuery(Expression<Func<T, object>> include)
        {
            IQueryable<T> entities = SetEntities<T>()
                .Include(include);

            return entities;
        }

        internal void RollBack()
        {
            if (_useTransaction)
            {
                if (_transactionType == TransactionTypes.DbTransaction)
                {
                    if (_transaction != null && _transaction.Connection != null)
                    {
                        _transaction.Rollback();

                        if (RepositoryBaseRollBackRaised != null)
                        {
                            RepositoryBaseRollBackRaised(_lastExecutedMethod);
                        }
                    }
                }
            }
        }

        internal void StartTransaction()
        {
            if (_useTransaction)
            {
                switch (_transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (_transaction == null || _transaction.Connection == null)
                            _transaction = _connection.BeginTransaction(_isolationLevel);
                        break;

                    case TransactionTypes.TransactionScope:
                        _transactionScope = new TransactionScope();
                        break;
                }
            }
        }

        protected void OnRepositoryBaseExceptionRaised(Exception e)
        {
            RepositoryBaseExceptionHandler handler = RepositoryBaseExceptionRaised;
            if (handler != null)
            {
                handler(e);
            }
        }

        public void Dispose()
        {
            CommitTransaction();

            _transaction = null;
            _transactionScope = null;

            // Check if this can be reverted. There was a problem with closing connections with MS ServiceLocator.
            /*if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection = null;
            }*/

            // Check if this can be deleted safely
            if (Model.Database.Connection != null && Model.Database.Connection.State != ConnectionState.Closed)
            {
                Model.Database.Connection.Close();
                Model.Database.Connection.Dispose();
            }

            if (!Object.Equals(Model, null))
            {
                Model.Dispose();
                Model = null;
            }
        }
    }
}
