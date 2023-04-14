using System;
using System.IO;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Velocity.Application;
using Velocity.Domain;
using Velocity.Infrastructure;

namespace Velocity.Api;

public class Startup
{
    private readonly AppSettings _settings;
    
    public Startup(IConfiguration configuration)
    {
        _settings = new AppSettings(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddControllers(options => options.Filters.Add<ProblemDetailsExceptionFilter>());
        services.AddCors(cors => cors.AddPolicy(
            "any", builder => builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Velocity Api"
            });
            
            var filePath = Path.Combine(AppContext.BaseDirectory, "Velocity.Api.xml");
            options.IncludeXmlComments(filePath);
        });
        services.AddAsyncInitialization();
        services.AddHealthChecks();
        services.AddInfrastructure(_settings.ConnectionString);
        services.AddDomain();
        services.AddApplication();
    }

    public void Configure(IApplicationBuilder app, IMigrator migrator)
    {
        migrator.Migrate();
        app.UseSerilogRequestLogging();
        app.UseCors("any");
        app.UseSwagger();
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Velocity Api"));
        app.UseReDoc(options =>
        {
            options.RoutePrefix = string.Empty;
            options.SpecUrl = "/swagger/v1/swagger.json";
            options.DocumentTitle = "Velocity Api";
        });
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers();
        });
    }
}