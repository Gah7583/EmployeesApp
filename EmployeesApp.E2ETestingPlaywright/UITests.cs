using Microsoft.Playwright;
using Xunit;

namespace EmployeesApp.E2ETestingPlaywright
{
    public class UITests : IClassFixture<PlaywrightFixture>
    {
        private IBrowser Browser { get; }

        public UITests(PlaywrightFixture fixture) 
        {
            Browser = fixture.Browser;  
        }

        [Fact]
        public async Task Clicking_CreateNewButton_Goes_To_Create()
        {

        }
    }
}
