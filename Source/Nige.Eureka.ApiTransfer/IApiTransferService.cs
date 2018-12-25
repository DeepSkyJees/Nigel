namespace Nige.Eureka.ApiTransfer
{
    public interface IApiTransferService
    {
        TService GetInstance<TService>() where TService : class;
    }
}