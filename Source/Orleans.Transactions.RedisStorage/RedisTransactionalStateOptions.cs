using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Transactions.RedisStorage
{
    public class RedisTransactionalStateOptions
    {
        public string ConnectionString { get; set; }

        public int DbIndex { get; set; } = 0;

        public int InitStage { get; set; } = DEFAULT_INIT_STAGE;
        public const int DEFAULT_INIT_STAGE = ServiceLifecycleStage.ApplicationServices;
    }
}
