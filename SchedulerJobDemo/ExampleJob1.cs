using Quartz;

namespace SchedulerJobDemo
{
    public class ExampleJob1 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"ExampleJob1 executed at {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
