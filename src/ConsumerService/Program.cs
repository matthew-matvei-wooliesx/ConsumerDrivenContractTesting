var httpClient = new HttpClient();
var upstreamServiceUrl = new Uri("http://localhost:5288");
httpClient.BaseAddress = upstreamServiceUrl;

var timeAccordingToService = await QueryTime(httpClient);
var clientUnderstandingOfTime = DateTimeOffset.UtcNow;

Console.WriteLine(
    "Time from service and client's understanding of the time are roughly the same: " +
    TimesAreRoughlyTheSame(DateTimeOffset.Parse(timeAccordingToService), clientUnderstandingOfTime)
    );

static async Task<string> QueryTime(HttpClient client)
{
    var response = await client.GetAsync("/time/now");
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadAsStringAsync();
}

static bool TimesAreRoughlyTheSame(DateTimeOffset startTime, DateTimeOffset endTime)
{
    var difference = startTime.Subtract(endTime);
    if (difference < TimeSpan.Zero)
    {
        difference = difference.Negate();
    }

    return difference < TimeSpan.FromSeconds(1);
}
