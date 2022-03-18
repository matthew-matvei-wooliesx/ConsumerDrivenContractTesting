using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Wow.Pact.Consumer;

namespace ConsumerService.Contracts
{
    public class ConsumerServiceFixture : WowPactConsumerFixture
    {
        public ConsumerServiceFixture() : base(Options)
        {
        }

        private static WowPactConsumerFixtureOptions Options => new()
        {
            ConsumerName = "ConsumerDrivenContractTesting.ConsumerService",
            ProviderName = "ConsumerDrivenContractTesting.UpstreamService",
            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }
        };
    }
}
