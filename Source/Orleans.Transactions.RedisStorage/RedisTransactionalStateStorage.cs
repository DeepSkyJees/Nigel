using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Orleans.Transactions.Abstractions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Transactions.RedisStorage
{
    public class RedisTransactionalStateStorage<TState> : ITransactionalStateStorage<TState>
        where TState : class, new()
    {
        private readonly string key;
        private readonly IDatabase database;
        private readonly JsonSerializerSettings jsonSettings;
        private readonly ILogger logger;

        private List<KeyValuePair<long, StateEntity>> states;
        public RedisTransactionalStateStorage(
            string key,
            IDatabase database,
            JsonSerializerSettings jsonSettings, 
            ILogger<RedisTransactionalStateStorage<TState>> logger)
        {
            this.key = key;
            this.jsonSettings = jsonSettings;
            this.logger = logger;
            this.jsonSettings.DefaultValueHandling = DefaultValueHandling.Include;
            this.database = database;
        }

        public Task<TransactionalStorageLoadResponse<TState>> Load()
        {
            throw new NotImplementedException();
        }

        public Task<string> Store(string expectedETag, TransactionalStateMetaData metadata, List<PendingTransactionState<TState>> statesToPrepare, long? commitUpTo, long? abortAfter)
        {
            throw new NotImplementedException();
        }

        void ReadState()
        {
            var results = new List<KeyValuePair<long, StateEntity>>();
            //var redis = this.connectionMultiplexer.conn
            //IDatabase db = 
        }
    }
}
