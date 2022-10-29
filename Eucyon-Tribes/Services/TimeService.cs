using Eucyon_Tribes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Tribes.Services
{
    public class TimeService : BackgroundService
    {
        public int tick;
        public System.Timers.Timer timer;
        private readonly IConfiguration _config;

        public TimeService(IConfiguration config)
        {
            _config = config;
        }

        public void OnTickEvent(Object source, ElapsedEventArgs t)
        {
            Console.WriteLine("game tick occured");
            tick++;
        }

        protected override Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
        {
            timer = new System.Timers.Timer
            {
                Interval = int.Parse(Environment.GetEnvironmentVariable("TribesGametickLength")) * 1000,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += OnTickEvent;
            return Task.CompletedTask;
        }
    }
}
