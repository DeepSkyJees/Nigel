using Newtonsoft.Json;
using Orleans.Transactions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Transactions.RedisStorage
{
    public class StateEntity
    {
        public string TransactionId { get; set; }

        public DateTime TransactionTimestamp { get; set; }

        public string TransactionManager { get; set; }

        public string StateJson { get; set; }

        public static StateEntity Create<T>(JsonSerializerSettings JsonSettings,
            string partitionKey, PendingTransactionState<T> pendingState)
            where T : class, new()
        {
            return new StateEntity
            {
                TransactionId = pendingState.TransactionId,
                TransactionTimestamp = pendingState.TimeStamp,
                TransactionManager = JsonConvert.SerializeObject(pendingState.TransactionManager, JsonSettings),
                StateJson = JsonConvert.SerializeObject(pendingState.State, JsonSettings)
            };
        }

        public T GetState<T>(JsonSerializerSettings JsonSettings)
        {
            return JsonConvert.DeserializeObject<T>(this.StateJson, JsonSettings);
        }
        public void SetState<T>(T state, JsonSerializerSettings JsonSettings)
        {
            StateJson = JsonConvert.SerializeObject(state, JsonSettings);
        }
    }
}
