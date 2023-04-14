using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Velocity.Api;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", true, true)
    .AddEnvironmentVariables()
    .Build();

AppSettings settings = new AppSettings(configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.WithProperty("app", "velocity-api")
    .WriteTo.Console()
    .CreateLogger();
    
try
{
    Log.Information("Serving...");
    var host = CreateHostBuilder(args).Build();
    await host.InitAsync();
    await host.RunAsync();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal Error");
    return 1;
}
finally
{
    Log.Information("Host terminated");
    Log.CloseAndFlush();
}

IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
        .UseSerilog();
}