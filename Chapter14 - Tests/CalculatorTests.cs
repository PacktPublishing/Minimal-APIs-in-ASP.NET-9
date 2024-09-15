using Chapter_14;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Chapter14Tests
{
    public class CalculatorTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public CalculatorTests(WebApplicationFactory<Program> applicationFactory) 
        { 
            _httpClient = applicationFactory.CreateClient();
        }
        
        [Fact]
        public void Sum_Test()
        {
            var calculatorService = new CalculatorService();
            int[] integers = { 1, 1, 8 };
            var result = calculatorService.Sum(integers);
            var expectedResult = 10;
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public async Task SumIntegers_ShouldReturnOk()
        {
            //Arrange
            var integers = new[] { 8, 4, 4 };
            var jsonContent = new StringContent(JsonSerializer.Serialize(integers), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync("/sumintegers", jsonContent);
            var result = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
