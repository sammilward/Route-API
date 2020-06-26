using System.Threading.Tasks;

namespace RouteAPI.RabbitMQ.Producer
{
    public interface IRouteAPIRabbitRPCService
    {
        Task<T> PublishRabbitMessageWaitForResponseAsync<T>(string method, object requestModel);
    }
}
