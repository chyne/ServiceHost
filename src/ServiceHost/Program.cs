namespace ServiceHost
{
    using Autofac;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac.Extensions.DependencyInjection;
    using FluentScheduler;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            var host = builder.Build();
            host.Run();
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddHostedService<Service>();
            services.Configure<HostOptions>(options => { options.ShutdownTimeout = TimeSpan.FromSeconds(10); });
            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<Sample>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<JobRegistry>().As<Registry>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class Service : IHostedService
    {
        private readonly IComponentContext _context;


        public Service(IComponentContext context, IHostApplicationLifetime lifetime)
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

    public class JobRegistry : Registry
    {
        public JobRegistry(IScheduledJob[] jobs)
        {
            foreach (IScheduledJob job in jobs)
            {
                var schedule = Schedule(job);
                job.Schedule(schedule);
            }
        }
    }

    public class Sample : IScheduledJob
    {
        public void Execute()
        {
            Console.WriteLine("Executing Sample.");

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public void Schedule(Schedule schedule)
        {
            schedule.NonReentrant().ToRunEvery(5).Seconds();
        }
    }

    public interface IScheduledJob : IJob
    {
        void Schedule(Schedule schedule);
    }

    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello!";
        }

    }
}
