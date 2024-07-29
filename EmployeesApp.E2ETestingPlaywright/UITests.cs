using FluentAssertions;
using Microsoft.Playwright;

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
            var page = await Browser.NewPageAsync();

            await page.GotoAsync("https://localhost:7019/");

            var createNew = page.Locator("text=Create New");
            await createNew.ClickAsync();
            page.Url.Should().Be("https://localhost:7019/Employees/Create/");
        }

        [Fact]
        public async Task Create_WhenSuccefullyExecuted_ReturnIndexView()
        {
            var page = await Browser.NewPageAsync();
            await page.GotoAsync("https://localhost:7019/Employees/Create/");

            await page.Locator("id=Name").FillAsync("Louis");
            await page.Locator("id=Age").FillAsync("25");
            await page.Locator("id=AccountNumber").FillAsync("123-1234567890-12");
            await page.Locator("id=Create").ClickAsync();

            page.Url.Should().Be("https://localhost:7019/");
        }
    }
}
