using Quartz;

namespace SchedulerJobDemo
{
    public class  ExampleJob2 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"ExampleJob2 executed at {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
