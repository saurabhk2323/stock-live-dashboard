using StockLiveDashboard.Contracts;
using StockLiveDashboard.Services;
using StockLiveDashboard.Services.DatabaseServices;
using StockLiveDashboard.WorkerService;
using StockLiveDashboard.WorkerService.Helpers;
using StockLiveDashboard.WorkerService.Hubs;
using StockLiveDashboard.WorkerService.Services;
using StockLiveDashboard.WorkerService.WorkerEngines;

public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>(); // Background service
                    services.AddHostedService<TradingEngine>(); // Long running job
                    services.AddSignalR();               // SignalR support
                    services.AddCors(options =>
                    {
                        options.AddDefaultPolicy(builder =>
                        {
                            builder
                                .WithOrigins("http://localhost:3000") // Your frontend's URL
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials(); // Required for SignalR
                        });
                    });
                    services.AddSingleton<ITradeQueue, TradeQueue>();
                    services.AddSingleton<CosmosDBClient>();
                    services.AddSingleton<CosmosDBService>();
                    services.AddSingleton<IStocksService, StocksService>();
                    services.AddSingleton<ITradesService, TradesService>();
                    services.AddSingleton<IStocksUpdateHelper, StocksUpdateHelper>();
                    services.AddSingleton<ITradesHelper, TradesHelper>();
                });

                webBuilder.Configure(app =>
                {
                    app.UseRouting();
                    app.UseCors();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<StockHub>("/stockshub");
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("Worker + SignalR is running.");
                        });
                    });
                });
            })
            .Build()
            .Run();
    }
}
