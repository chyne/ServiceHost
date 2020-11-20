namespace ServiceHostFramework
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Main;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>();

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
            services.AddMvcCore(options => { options.EnableEndpointRouting = false; }).AddApplicationPart(typeof(StatusController).Assembly).AddControllersAsServices();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<MainModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}
