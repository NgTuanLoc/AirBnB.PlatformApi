using FluentAssertions;

namespace Test.Controllers
{
    public class LocationControllerTest : IClassFixture<TestApiWebFactory>
    {
        private readonly HttpClient _client;
        public LocationControllerTest(TestApiWebFactory factory)
        {
            _client = factory.CreateClient();
        }
        [Fact]
        public async Task GetAllLocationListShouldReturnEmptyList()
        {
            HttpResponseMessage response = await _client.GetAsync("api/v1/location");
            string responseBody = await response.Content.ReadAsStringAsync();
            response.Should().BeSuccessful();
        }
    }
}