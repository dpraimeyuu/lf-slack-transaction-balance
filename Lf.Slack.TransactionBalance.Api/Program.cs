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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();

