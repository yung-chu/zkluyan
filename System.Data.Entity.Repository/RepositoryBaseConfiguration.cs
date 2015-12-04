using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace System.Data.Entity.Repository
{
    public class RepositoryBaseConfiguration
    {
        public TransactionTypes TransactionType = TransactionTypes.DbTransaction;
        public IsolationLevel IsolationLevel = IsolationLevel.ReadUncommitted;

        public bool ProxyCreationEnabled = false;
        public bool RethrowExceptions = false;
        public bool UseTransaction = false;
        public bool LazyConnection = false;
        public bool SaveLastExecutedMethodInfo = false;

        public int CommandTimeout = 300;

        public string ConnectionString = string.Empty;
    }
}
