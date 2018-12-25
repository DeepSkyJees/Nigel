using DHaven.Faux;

namespace Nige.Eureka.ApiTransfer
{
    public class ApiTransferService : IApiTransferService
    {
        private readonly FauxCollection _collection;

        public ApiTransferService()
        {
            _collection = new FauxCollection(typeof(ApiTransferService));
        }

        public TService GetInstance<TService>() where TService : class
        {
            var ts = _collection.GetInstance<TService>();
            return ts;
        }
    }
}