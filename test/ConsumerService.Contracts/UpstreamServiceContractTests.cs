using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using PactNet.Matchers;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace ConsumerService.Contracts
{
    public class UpstreamServiceContractTests : IClassFixture<ConsumerServiceFixture>
    {
        private readonly HttpClient _mockClient;
        private readonly IMockProviderService _mockProvider;

        public UpstreamServiceContractTests(ConsumerServiceFixture fixture)
        {
            _mockClient = fixture.MockHttpClient;
            _mockProvider = fixture.MockProviderService;
            _mockProvider.ClearInteractions();
        }

        [Fact(DisplayName = "When getting the time now Then the returned time should be parseable as a DateTimeOffset")]
        public async void GetTimeNowReturnsTimeParseableAsDateTimeOffset()
        {
            // Arrange
            _mockProvider
                .UponReceiving("A request to get the current time")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/time/now"
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "text/plain; charset=utf-8" }
                    },
                    Body = Match.Regex(DateTimeOffset.UtcNow.ToString(), @"^(?!\s*$).+")
                });

            // Act
            var timeNow = await QueryTime();
            var exception = Record.Exception(() => DateTimeOffset.Parse(timeNow));

            // Assert
            Assert.Null(exception);
        }

        private async Task<string> QueryTime()
        {
            var response = await _mockClient.GetAsync("/time/now");

            return await response.Content.ReadAsStringAsync();
        }
    }
}
