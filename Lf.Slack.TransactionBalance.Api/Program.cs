using Lf.Slack.TransactionBalance.Api;
using Lf.Slack.TransactionBalance.Application;
using Lf.Slack.TransactionBalance.Domain;
using Lf.Slack.TransactionBalance.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<TransactionBalanceService>();

var dbConnectionString = builder.Configuration.GetConnectionString("LF_SLACK_CONNECTION_STRING");
builder.Services.AddTransient<ITransactionBalanceRepository, TransactionBalanceRelationalRepository>(_ => new TransactionBalanceRelationalRepository($"Data Source={dbConnectionString}"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment() || builder.Environment.WithSwagger())
{
    app.Logger.LogInformation("Running with Swagger");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "api-docs";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();