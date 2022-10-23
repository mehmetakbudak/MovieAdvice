using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieAdvice.Repository;
using MovieAdvice.Repository.Repositories;
using MovieAdvice.Service.Services;
using MovieAdvice.Worker;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddHostedService<Worker>();

}).ConfigureServices((hostContext, services) =>
{
    services.AddDbContext<MovieContext>(options =>
        options.UseSqlServer(hostContext.Configuration.GetConnectionString("AppConnectionString"),
        options => options.EnableRetryOnFailure()));

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IMovieService, MovieService>();
    services.AddScoped<IMovieRepository, MovieRepository>();


}).Build();

await host.RunAsync();
