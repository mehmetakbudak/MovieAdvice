using MovieAdvice.Repository;
using Microsoft.EntityFrameworkCore;
using MovieAdvice.Repository.Repositories;
using MovieAdvice.Service.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<MovieAdviceContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString"),
        options => options.EnableRetryOnFailure()));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();


builder.Services.AddMvc().AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore).ConfigureApiBehaviorOptions(options =>
{

}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});


var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
