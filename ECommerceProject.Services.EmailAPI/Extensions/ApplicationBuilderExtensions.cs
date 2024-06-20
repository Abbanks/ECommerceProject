using ECommerceProject.Services.EmailAPI.Messaging;

namespace ECommerceProject.Services.EmailAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IEventBridgeConsumer EventBridgeConsumer { get; set; }

        public static IApplicationBuilder UseEventBridgeConsumer(this IApplicationBuilder app)
        {
            EventBridgeConsumer = app.ApplicationServices.GetService<IEventBridgeConsumer>();
            var hostApplicationLife = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLife.ApplicationStarted.Register(OnStart);
            hostApplicationLife.ApplicationStopping.Register(OnStop);

            return app;
        }

        private static void OnStart()
        {
            EventBridgeConsumer.Start().GetAwaiter().GetResult(); // Ensure it waits for start to complete
        }

        private static void OnStop()
        {
            EventBridgeConsumer.Stop().GetAwaiter().GetResult(); // Ensure it waits for stop to complete
        }
    }


}
