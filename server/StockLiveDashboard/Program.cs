using StockLiveDashboard.Contracts;
using StockLiveDashboard.Services;
using StockLiveDashboard.Services.DatabaseServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<CosmosDBClient>();

// register services
builder.Services.AddScoped<CosmosDBService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IInvestorsService, InvestorsService>();
builder.Services.AddScoped<ITradesService, TradesService>();

builder.Services.AddControllers();

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
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();
