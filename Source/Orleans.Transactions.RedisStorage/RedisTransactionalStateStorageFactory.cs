using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Orleans.Configuration;
using Orleans.Runtime;
using Orleans.Transactions.Abstractions;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orleans.Transactions.RedisStorage
{
    public class RedisTransactionalStateStorageFactory : ITransactionalStateStorageFactory, ILifecycleParticipant<ISiloLifecycle>
    {
        private readonly string name;
        private readonly RedisTransactionalStateOptions options;
        private readonly ClusterOptions clusterOptions;
        private readonly JsonSerializerSettings jsonSettings;
        private readonly ILogger<RedisTransactionalStateStorageFactory> logger;
        private IDatabase database;

        public static ITransactionalStateStorageFactory Create(IServiceProvider services, string name)
        {
            IOptionsSnapshot<RedisTransactionalStateOptions> optionsSnapshot = services.GetRequiredService<IOptionsSnapshot<RedisTransactionalStateOptions>>();
            return ActivatorUtilities.CreateInstance<RedisTransactionalStateStorageFactory>(services, name, optionsSnapshot.Get(name));
        }

        public RedisTransactionalStateStorageFactory(string name,
            RedisTransactionalStateOptions options,
            IOptions<ClusterOptions> clusterOptions,
            ITypeResolver typeResolver,
            IGrainFactory grainFactory,
            ILogger<RedisTransactionalStateStorageFactory> logger)
        {
            this.name = name;
            this.options = options;
            this.clusterOptions = clusterOptions.Value;
            this.jsonSettings = TransactionalStateFactory.GetJsonSerializerSettings(
                typeResolver,
                grainFactory);
            this.logger = logger;
        }

        public ITransactionalStateStorage<TState> Create<TState>(string stateName, IGrainActivationContext context) where TState : class, new()
        {
            var redisKey = MakeRedisKey(context, stateName);
            return ActivatorUtilities.CreateInstance<RedisTransactionalStateStorage<TState>>(context.ActivationServices, redisKey, this.database, this.jsonSettings);
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe(OptionFormattingUtilities.Name<RedisTransactionalStateStorageFactory>(this.name), this.options.InitStage, Init);
        }

        private string MakeRedisKey(IGrainActivationContext context, string stateName)
        {
            string grainKey = context.GrainInstance.GrainReference.ToShortKeyString();
            var key = $"{grainKey}_{this.clusterOptions.ServiceId}_{stateName}";
            return key;
        }
        private async Task CreateDatabase()
        {
            this.logger.LogInformation("connect redis server");
            ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(this.options.ConnectionString);
            if (connectionMultiplexer.IsConnected)
            {
                this.database = connectionMultiplexer.GetDatabase(this.options.DbIndex);
                await Task.CompletedTask;
            }
            else
            {

                throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, "redis server error");
            }
        }


        private Task Init(CancellationToken cancellationToken)
        {
            return CreateDatabase();
        }
    }
}
