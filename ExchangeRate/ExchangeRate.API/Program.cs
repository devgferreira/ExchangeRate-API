using DotNetEnv;
using ExchangeRate.Application.Settings;
using ExchangeRate.Infra.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Env.Load();
builder.Services.AddControllers();
builder.Services.AddInfrastructureAPI(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

var appSettings = new ApplicationSettings
{
    URLAwesomeAPI = Environment.GetEnvironmentVariable("URL_AWESOME_API")!
};

builder.Services.AddSingleton<IApplicationSettings>(appSettings);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
