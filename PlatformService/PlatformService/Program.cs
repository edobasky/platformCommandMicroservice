using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
opt.UseInMemoryDatabase("InMem"));
builder.Services.AddScoped<IPlatformRepo,PlatformformRepo>();
builder.Services.AddHttpClient<ICommanDataClient,HttpCommanDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var environment = builder.Environment;
Console.WriteLine($"Current Environment: {environment.EnvironmentName}");
Console.WriteLine($"CommandService: {builder.Configuration["CommandService"]}");
Console.WriteLine($"---> CommandService Endpoint {builder.Configuration["CommandService"]}{builder.Configuration["CommandPlatformUrl"]}");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PrepDb.PrepPopulation(app);

app.Run();
