using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarRental.API.Services;

namespace CarRental.API.BackgroundServices
{
    public class ReservationStatusUpdater : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Timer _timer;

        public ReservationStatusUpdater(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _timer = new Timer(UpdateReservationStatus, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private void UpdateReservationStatus(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rentalHistoryRepository = scope.ServiceProvider.GetRequiredService<IRentalHistoryRepository>();

                // Use rentalHistoryRepository as needed within this scope
                rentalHistoryRepository.UpdateActiveReservations().Wait();
                rentalHistoryRepository.UpdateCompletedReservations().Wait();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
