using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace UKParliament.CodeTest.Data.Tests;
internal class PersonManagerContextTests
{
    [Test]
    public async Task Context_ShouldInitialiseAndConnectSuccessfully()
    {
        var options = new DbContextOptionsBuilder<PersonManagerContext>()
            .UseInMemoryDatabase(databaseName: "SmokeTestDb")
            .Options;

        await using var context = new PersonManagerContext(options);

        await context.Database.EnsureCreatedAsync();
        (await context.People.CountAsync()).ShouldBeGreaterThanOrEqualTo(0);
    }
}
