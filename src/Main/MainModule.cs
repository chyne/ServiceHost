namespace Main
{
    using Autofac;
    using FluentScheduler;

    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SampleJob>().AsSelf().AsImplementedInterfaces();
            builder.RegisterType<JobRegistry>().As<Registry>();
        }
    }
}