using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace SchedulerJobDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ISchedulerFactory _schedulerFactory;

        public ScheduleController(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        [HttpPost("update-schedule")]
        public async Task<IActionResult> UpdateSchedule(string jobName, string cronExpression)
        {
            var scheduler = await _schedulerFactory.GetScheduler();
            var jobKey = new JobKey(jobName);

            var newTrigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .WithIdentity($"{jobName}-trigger")
                .StartNow()
                .WithCronSchedule(cronExpression)
                .Build();

            await scheduler.RescheduleJob(new TriggerKey($"{jobName}-trigger"), newTrigger);

            return Ok("Schedule updated successfully.");
        }
    }
}