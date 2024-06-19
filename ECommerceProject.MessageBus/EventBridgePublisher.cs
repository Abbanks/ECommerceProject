using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using Newtonsoft.Json;

namespace ECommerceProject.EventBridge
{
    public class EventBridgePublisher : IEventPublisher
    {
        private readonly IAmazonEventBridge _eventBridgeClient;
        private readonly string _eventBus;

        public EventBridgePublisher(IAmazonEventBridge eventBridgeClient, string eventBus)
        {
            _eventBridgeClient = eventBridgeClient;
            _eventBus = eventBus;
        }

        public async Task PublishEvent(object detail, string detailType, string source)
        {
            var jsonDetail = JsonConvert.SerializeObject(detail);

            var putEventsRequest = new PutEventsRequest
            {
                Entries = new List<PutEventsRequestEntry>
            {
                new PutEventsRequestEntry
                {
                    EventBusName = _eventBus,
                    Source = source,
                    DetailType = detailType,
                    Detail = jsonDetail,
                    Time = DateTime.UtcNow
                }
            }
            };

            var response = await _eventBridgeClient.PutEventsAsync(putEventsRequest);

            if (response.FailedEntryCount > 0)
            {
                // Handle failures
                Console.WriteLine("Failed to publish some events.");
            }
            else
            {
                Console.WriteLine("Event published successfully.");
            }
        }
    }
}
