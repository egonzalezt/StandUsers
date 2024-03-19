using StandUsers.WebApi.Extensions;
using StandUsers.WebApi.Middlewares;
using StandUsers.Infrastructure.EntityFrameworkCore.DbContext;
using StandUsers.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);
builder.Services.Configure<OperatorIdentification>(builder.Configuration.GetSection("OperatorInformation"));
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.AddSwaggerUi();

app.UseMiddleware<BusinessExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("X-Pagination-Total-Pages")
           .WithExposedHeaders("X-Pagination-Next-Page")
           .WithExposedHeaders("X-Pagination-Has-Next-Page")
           .WithExposedHeaders("X-Pagination-Total-Pages");
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StandUsersDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    dbContext.Database.EnsureCreated();
    await dbContext.Database.MigrateAsync();
    logger.LogInformation("Database created successfully or already exists.");
}

app.Run();
