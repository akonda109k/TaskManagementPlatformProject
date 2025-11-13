using ReportingService.Services;
using ReportingService.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IReportingService, ReportingsService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStubAuth();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
