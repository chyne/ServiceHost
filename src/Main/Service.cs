namespace Main
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using FluentScheduler;
    using Microsoft.Extensions.Hosting;

    public class Service : IHostedService
    {
        private readonly IComponentContext _context;


        public Service(IComponentContext context, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        {
            _context = context;

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);
            lifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            JobManager.Initialize(_context.Resolve<Registry>());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            JobManager.StopAndBlock();

            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Console.WriteLine("OnStarted");
        }

        private void OnStopping()
        {
            Console.WriteLine("OnStopping");
        }

        private void OnStopped()
        {
            Console.WriteLine("OnStopped");
        }
    }
}