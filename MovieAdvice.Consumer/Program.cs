using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieAdvice.Consumer;
using MovieAdvice.Infrastructure.Configurations;
using MovieAdvice.Repository;
using MovieAdvice.Service;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddHostedService<Worker>();

}).ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<MovieAdviceContext>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("AppConnectionString"),
        options => options.EnableRetryOnFailure()));    

    IOCConfiguration.Initialize(services);

    ConsumerConfiguration.Initialize(hostContext.Configuration);
}).Build();

await host.RunAsync();
