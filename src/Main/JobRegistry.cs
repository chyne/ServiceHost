namespace Main
{
    using FluentScheduler;

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
}