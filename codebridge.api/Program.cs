using codebridge.api;
using codebridge.api.Data;
using codebridge.api.middlewares;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options=> 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddControllers();
builder.Services.AddHostedService<TestData>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();

//app.MapGet("/ping", () =>
//{
//    Results.Text("Dogs house service.Version 1.0.1");
//});

//app.MapGet("/dogs", async ([FromQuery] GetDogsRequest params1, IMediator mediator) =>
//{
//    await Task.Delay(TimeSpan.FromMinutes(5));
//    var response = await mediator.Send(params1);
//    return Results.Ok(response);
//});

app.Run();