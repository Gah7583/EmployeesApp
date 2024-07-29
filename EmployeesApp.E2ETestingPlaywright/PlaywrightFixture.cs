using Microsoft.Playwright;

namespace EmployeesApp.E2ETestingPlaywright
{
    [CollectionDefinition(nameof(PlaywrightFixture))]
    public class SharedPlaywrightCollection : ICollectionFixture<PlaywrightFixture> { }

    public class PlaywrightFixture : IAsyncLifetime
    {
        public async Task InitializeAsync()
        {
            _playwrightInstance = await Playwright.CreateAsync();
            Browser = await _playwrightInstance.Chromium.LaunchAsync();
        }

        public IBrowser Browser { get; set; } = null!;
        private IPlaywright _playwrightInstance { get; set; } = null!;

        public async Task DisposeAsync()
        {
            await Browser.DisposeAsync();
            _playwrightInstance.Dispose();
        }
    }
}
