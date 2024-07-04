using FluentAssertions;

namespace EmployeesApp.IntegrationTests
{
    public class EmployeesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _httpClient;

        public EmployeesControllerIntegrationTests(TestingWebAppFactory<Program> factory) => _httpClient = factory.CreateClient();

        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            var response = await _httpClient.GetAsync("/Employees");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().ContainAll("Ada","Eve");
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreateForm()
        {
            var response = await _httpClient.GetAsync("/Employees/Create");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().Contain("Please provide a new employee data");
        }

        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessaged()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");

            var formModel = new Dictionary<string, string>
            {
                {"Name", "New Employee"},
                {"Age", "25"}
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _httpClient.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().Contain("Account number is required");
        }

        [Fact]
        public async Task Create_WhenPOSTExecuted_ReturnsToIndexViewWithCreatedEmployee()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create");

            var formModel = new Dictionary<string, string>
            {
                {"Name", "New Employee"},
                {"Age", "25"},
                {"AccountNumber", "123-1234567890-12"}
            };

            postRequest.Content = new FormUrlEncodedContent(formModel);

            var response = await _httpClient.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().ContainAll("New Employee", "123-1234567890-12");
        }
    }
}