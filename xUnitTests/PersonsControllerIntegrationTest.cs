using FluentAssertions;

namespace xUnitTests
{
    public class PersonsControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public PersonsControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        #region Index Tests
        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            //Arrange
            //Act
            HttpResponseMessage httpResponseMessage = await _client.GetAsync("/Persons/Index");
            //Assert
            httpResponseMessage.EnsureSuccessStatusCode();
        }
        #endregion
    }
}
