
using WebApplication2.Models;

namespace WebApplication2.BackgroundServices
{
    public class DatabaseBackGround : BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseBackGround(ILogger <BackgroundService> logger, IServiceProvider serviceProvider)
        {
             _logger = logger;
            _serviceProvider = serviceProvider;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
              while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<ToDoContext>();
                       
                    var p =  ctx.CartItems.ToList();

                      foreach (var items in p)
                    {
                        await Console.Out.WriteLineAsync(items.item?.ToString());
                        if (items.item?.ToString() == "Tofu") {
                            await Console.Out.WriteLineAsync("send and email here if values are low");
                        }
                    } 
                }

                _logger.LogInformation("hello my dude");
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
