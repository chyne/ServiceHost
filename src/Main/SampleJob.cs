using System;

namespace Main
{
    using System.Threading;
    using FluentScheduler;

    public class SampleJob : IScheduledJob
    {
        public void Execute()
        {
            Console.WriteLine($"Executing {nameof(SampleJob)}.");

            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        public void Schedule(Schedule schedule)
        {
            schedule.NonReentrant().ToRunEvery(5).Seconds();
        }
    }
}
