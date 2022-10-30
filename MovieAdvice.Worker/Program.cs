using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieAdvice.Infrastructure.Configurations;
using MovieAdvice.Repository;
using MovieAdvice.Service;
using MovieAdvice.Worker;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddHostedService<Worker>();

}).ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<MovieAdviceContext>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("AppConnectionString"),
        options => options.EnableRetryOnFailure()));

    WorkerConfiguration.Initialize(hostContext.Configuration);

    IOCConfiguration.Initialize(services);

}).Build();

await host.RunAsync();
