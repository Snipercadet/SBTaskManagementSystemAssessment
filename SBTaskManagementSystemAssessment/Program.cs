using Microsoft.AspNetCore.Mvc;
using SBTaskManagementSystemAssessment.Extensions;
using SBTaskManagementSystemAssessment.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterInfrastructure(builder.Configuration);
//builder.Services.AddHostedService<TaskProcessingWorker>();
builder.Services.AddControllers();
var contextAccessor = builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseErrorHandler();
app.MapControllers();
app.UseWebHelper();
app.Run();
