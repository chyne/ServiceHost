namespace Main
{
    using FluentScheduler;

    public interface IScheduledJob : IJob
    {
        void Schedule(Schedule schedule);
    }
}