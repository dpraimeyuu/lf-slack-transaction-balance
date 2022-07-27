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
builder.Services.AddTransient<ITransactionBalanceRepository, TransactionBalanceRelationalRepository>(_ => new TransactionBalanceRelationalRepository("Data Source=./bin/Debug/net6.0/lfslack.db"));

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

