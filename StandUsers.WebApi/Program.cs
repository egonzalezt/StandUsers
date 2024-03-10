using StandUsers.WebApi.Extensions;
using StandUsers.WebApi.Middlewares;
using StandUsers.Infrastructure.EntityFrameworkCore.DbContext;
using StandUsers.Domain.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);
builder.Services.Configure<OperatorIdentification>(builder.Configuration.GetSection("OperatorInformation"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseStaticFiles();
app.AddSwaggerUi();

app.UseMiddleware<BusinessExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StandUsersDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    dbContext.Database.EnsureCreated();
    logger.LogInformation("Database created successfully or already exists.");
}

app.Run();
