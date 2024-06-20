using Amazon.EventBridge;
using ECommerceProject.Services.EmailAPI.Message;
using ECommerceProject.Services.EmailAPI.Models.Dto;
using ECommerceProject.Services.EmailAPI.Services;
using Newtonsoft.Json;

namespace ECommerceProject.Services.EmailAPI.Messaging
{
    public class EventBridgeConsumer : IEventBridgeConsumer
    {
        private readonly IAmazonEventBridge _eventBridgeClient;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly string _eventBusName;

        public EventBridgeConsumer(IConfiguration configuration, EmailService emailService, IAmazonEventBridge eventBridgeClient)
        {
            _emailService = emailService;
            _configuration = configuration;
            _eventBridgeClient = eventBridgeClient;
            _eventBusName = _configuration.GetValue<string>("EventBridgeDetails:EventBusName");
        }

        public async Task ProcessEvent(string detailType, string eventBody)
        {
            Console.WriteLine($"Processing event of type: {detailType}");
            Console.WriteLine($"Event Body: {eventBody}");

            switch (detailType)
            {
                case "EmailCart":
                    await OnEmailCartRequestReceived(eventBody);
                    break;
                case "OrderPlaced":
                    await OnOrderPlacedRequestReceived(eventBody);
                    break;
                case "UserRegister":
                    await OnUserRegisterRequestReceived(eventBody);
                    break;
                default:
                    Console.WriteLine($"Unhandled event type: {detailType}");
                    break;
            }
        }

        public Task Start()
        {
            // Start listening for events (implementation depends on your setup)
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            // Stop listening for events (implementation depends on your setup)
            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceived(string eventBody)
        {
            try
            {
                // Assuming the JSON data's "detail" property needs to be deserialized into CartDto
                var eventWrapper = JsonConvert.DeserializeObject<EventWrapper<CartDto>>(eventBody);
                if (eventWrapper?.Detail == null)
                {
                    Console.WriteLine("Failed to deserialize CartDto from event body");
                    return;
                }

                await _emailService.EmailCartAndLog(eventWrapper.Detail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing email cart request: {ex.Message}");
            }
        }

        private async Task OnOrderPlacedRequestReceived(string eventBody)
        {
            try
            {
                var objMessage = JsonConvert.DeserializeObject<RewardsMessage>(eventBody);
                if (objMessage == null)
                {
                    Console.WriteLine("Failed to deserialize RewardsMessage from event body");
                    return;
                }

                await _emailService.LogOrderPlaced(objMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing order placed request: {ex.Message}");
            }
        }

        private async Task OnUserRegisterRequestReceived(string eventBody)
        {
            try
            {
                var email = JsonConvert.DeserializeObject<string>(eventBody);
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Failed to deserialize email from event body");
                    return;
                }

                await _emailService.RegisterUserEmailAndLog(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing user register request: {ex.Message}");
            }
        }
    }


}
