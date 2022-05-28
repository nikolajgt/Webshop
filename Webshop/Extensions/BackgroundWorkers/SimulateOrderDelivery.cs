

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webshop.Hubs;
using Webshop.Interface;
using Webshop.Models;

namespace Webshop.Extensions.BackgroundWorkers
{
    public class SimulateOrderDelivery : BackgroundService
    {
        private int executionCount = 2000;
        private readonly ILogger<SimulateOrderDelivery> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<NotificationsHub, INotificiationsHub> _hub;

        public SimulateOrderDelivery(ILogger<SimulateOrderDelivery> logger, IServiceScopeFactory scopeFactory, IHubContext<NotificationsHub, INotificiationsHub> hub)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _hub = hub;

        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SimulateOrderDelivery worker started up.");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {

            cancellationToken.Register(() =>
                _logger.LogInformation($"SimulateOrderDelivery is stopping service."));

            while (!cancellationToken.IsCancellationRequested)
            {
                //10 sec before running code
                await Task.Delay(10000);
                _logger.LogInformation("SimulateOrderDelivery work has begun.");
                await DoWork();
                await Task.Delay(executionCount, cancellationToken);
            }

            _logger.LogInformation("SimulateOrderDelivery has stopped");
        }


        //Here is what the backgroundworker is doing
        //The idea of the function is it takes every order from ordered table and simulating
        //the delivery with some random generated numbers and using signalr hub to 
        //to send it to frontend
        private async Task DoWork()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
                var list = await dbContext.Orders.Include(x => x.Customer).ToListAsync();

                if (list.Any())
                {
                    Random r = new Random();
                    int fromCompanyToDeliveryGuy = r.Next(1000, 20000);
                    await Task.Delay(fromCompanyToDeliveryGuy);
                    foreach (var o in list)
                    {
                        if (!String.IsNullOrEmpty(o.Customer.Id.ToString()))
                            await SendNotification(o.Customer.Id.ToString(), $"Time: {fromCompanyToDeliveryGuy}  Package given to delivery company");
                    }

                    int fromDeliveryGuyToUser = r.Next(1000, 20000);
                    await Task.Delay(fromDeliveryGuyToUser);

                    foreach (var o in list)
                    {
                        if (!String.IsNullOrEmpty(o.Customer.Id.ToString()))
                            await SendNotification(o.Customer.Id.ToString(), $"Time: {fromDeliveryGuyToUser}  Package has arrived");
                        o.IsDelivered = true;

                        try
                        {
                            dbContext.Orders.Update(o);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Worker failed: {ex.Message}");
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SendNotification(string userid, string message)
        {
            await _hub.Clients.Groups(userid).SendNotificationsOut(message);
        }

    }
}
