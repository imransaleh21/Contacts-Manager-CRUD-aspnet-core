using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
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

            // Below code is to verify whether the response body contains the table with class personsList for intregration testing
            string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            HtmlDocument htmlDocument = new();
            htmlDocument.LoadHtml(responseBody);

            var document = htmlDocument.DocumentNode;
            document.QuerySelectorAll("table.personsList").Should().NotBeNull();
        }
        #endregion
    }
}
