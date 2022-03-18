using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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

        private static WowPactVerifierOptions Options => new()
        {
            ProviderName = "ConsumerDrivenContractTesting.UpstreamService",
            ProviderUri = "http://localhost:5288",
            UseProviderStates = true
        };

        [Fact]
        public async Task VerifyConsumerDrivenContracts()
        {
            var pathToLocalConsumerPact = Path.Join(
                "..",
                "..",
                "..",
                "..",
                "ConsumerService.Contracts",
                "bin",
                "Debug",
                "net6.0",
                "pacts",
                "consumerdrivencontracttesting.consumerservice-consumerdrivencontracttesting.upstreamservice.json");

            // Workaround for issue with PactNet lib (https://github.com/pact-foundation/pact-net/issues/330)
            Environment.SetEnvironmentVariable("PACT_DISABLE_SSL_VERIFICATION", "true");

            await RunAsync(() => _pactVerifier.VerifyContract("ConsumerService", pathToLocalConsumerPact));
        }

        private static async Task RunAsync(Func<Task> task)
        {
            using var host = BuildMockProvider();
            await host.StartAsync();
            await task();
            await host.StopAsync();
        }

        private static IWebHost BuildMockProvider() => WebHost.CreateDefaultBuilder()
            .UseUrls(Options.ProviderUri)
            .UseStartup<Startup>()
            .ConfigureTestServices(services =>
            {
                services.AddSingleton(new ProviderStatesContainer(config =>
                {
                    config.Add("", () => { });
                }));
                services.AddSingleton<IStartupFilter, ProviderStatesStartupFilter>();
            })
            .Build();
    }
}
