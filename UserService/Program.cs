using UserService.Services;
using UserService.Middleware;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services  
builder.Services.AddSingleton<IUserService, UsersService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserService API", Version = "v1" });
});

var app = builder.Build();

// Use stub authentication middleware  
app.UseStubAuth();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
