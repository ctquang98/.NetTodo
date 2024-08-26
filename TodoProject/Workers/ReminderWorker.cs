
using TodoProject.Services;

namespace TodoProject.Workers
{
    public class ReminderWorker : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly CardService cardService;

        public ReminderWorker(CardService cardService)
        {
            this.cardService = cardService;
        }

        private async void SendMessage(object state)
        {
            var cards = await cardService.GetCardBeforeDate(DateTime.Now.AddMonths(2));
            foreach (var card in cards)
            {
                Console.WriteLine($"{card.Name} duedate: {card.DueDate}");
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(SendMessage, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
