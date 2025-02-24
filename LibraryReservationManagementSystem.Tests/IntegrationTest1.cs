namespace LibraryReservationManagementSystem.Tests
{
    public class IntegrationTest1
    {
        [Test]
        public async Task GetWebResourceRootReturnsOkStatusCode()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.LibraryReservationManagementSystem_AppHost>();
            appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });
            await using var app = await appHost.BuildAsync();
            var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("libraryreservationmanagementsystem");
            await resourceNotificationService.WaitForResourceAsync("libraryreservationmanagementsystem", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
            var response = await httpClient.GetAsync("/health");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
