using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Web.Startup;

namespace UKParliament.CodeTest.Web.Tests.Startup;

[TestFixture]
public class ServiceCollectionExtensionsTests
{
    private ServiceCollection _services;

    [SetUp]
    public void Setup()
    {
        _services = new ServiceCollection();
        _services.AddApplicationServices();
    }

    [Test]
    public void AddApplicationServices_RegistersRepositoriesWithCorrectLifetime()
    {
        // Act & Assert
        _services.ShouldContain(service =>
            service.ServiceType == typeof(IPersonRepository) &&
            service.ImplementationType == typeof(PersonRepository) &&
            service.Lifetime == ServiceLifetime.Scoped);

        _services.ShouldContain(service =>
            service.ServiceType == typeof(IDepartmentRepository) &&
            service.ImplementationType == typeof(DepartmentRepository) &&
            service.Lifetime == ServiceLifetime.Scoped);
    }
}