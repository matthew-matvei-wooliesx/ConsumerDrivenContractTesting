using System;
using System.Threading.Tasks;
using Wow.Pact.Provider;
using Xunit;
using Xunit.Abstractions;

namespace UpstreamService.Contracts
{
    public class ConsumerDrivenContractTests
    {
        private readonly WowPactVerifier _pactVerifier;

        public ConsumerDrivenContractTests(ITestOutputHelper output)
        {
            _pactVerifier = new WowPactVerifier(Options, output);
        }

        private static WowPactVerifierOptions Options => new WowPactVerifierOptions()
        {
            ProviderName = "ConsumerDrivenContractTesting.UpstreamService",
            ProviderUri = "http://localhost:5288",
            UseProviderStates = true
        };

        [Fact]
        public async Task VerifyConsumerDrivenContracts()
        {
            if (TestTriggeredByWebhook(out var pactUri, out var consumerName))
            {
            }
        }

        private static bool TestTriggeredByWebhook(out string pactUri, out string consumerName)
        {
            consumerName = default;

            return EnvironmentVariableDefined("PACTBROKER_PACT_URI", out pactUri) &&
                EnvironmentVariableDefined("PACTBROKER_CONSUMER_NAME", out consumerName);
        }

        private static bool EnvironmentVariableDefined(
            string environmentVariableName,
            out string environmentVariableValue)
        {
            environmentVariableValue = Environment.GetEnvironmentVariable(environmentVariableName) ?? "";

            return !string.IsNullOrEmpty(environmentVariableValue);
        }
    }
}
