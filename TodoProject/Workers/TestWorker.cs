
namespace TodoProject.Workers
{
    public class TestWorker : IHostedService, IDisposable
    {
        private Timer timer;
        private int count = 0;

        private void HandleData(object state)
        {
            Console.WriteLine("Worker running: " + DateTime.Now);
            if (++count == 5) StopAsync(new CancellationToken());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Worker started: " + DateTime.Now);
            timer = new Timer(HandleData, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Worker stopped: " + DateTime.Now);
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
