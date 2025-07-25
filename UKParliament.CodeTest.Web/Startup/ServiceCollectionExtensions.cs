using FluentValidation;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Mappers;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.Validators;
using UKParliament.CodeTest.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Web.Startup;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDbContext<PersonManagerContext>(op => op.UseInMemoryDatabase("PersonManager"));
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IPersonServiceMapper, PersonServiceMapper>();
        services.AddScoped<IPersonApiMapper, PersonApiMapper>();
        services.AddScoped<IValidator<PersonViewModel>, PersonViewModelValidator>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IDepartmentServiceMapper, DepartmentServiceMapper>();

        return services;
    }
}
