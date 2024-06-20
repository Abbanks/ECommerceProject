namespace ECommerceProject.Services.EmailAPI.Messaging
{
    public interface IEventBridgeConsumer
    {
        Task Start();
        Task Stop();
        
    }
}
