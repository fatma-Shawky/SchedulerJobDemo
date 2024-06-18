using Quartz;
using SchedulerJobDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Load schedule configuration
var scheduleConfig = builder.Configuration.GetSection("ScheduleConfig").Get<ScheduleConfig>();

// Register Quartz.NET services
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Define the first job and tie it to the ExampleJob1 class
    var jobKey1 = new JobKey("ExampleJob1");
    q.AddJob<ExampleJob1>(opts => opts.WithIdentity(jobKey1));

    // Trigger the first job using a cron schedule from the configuration
    q.AddTrigger(opts => opts
        .ForJob(jobKey1)
        .WithIdentity("ExampleJob1-trigger")
        .StartNow()
        .WithCronSchedule(scheduleConfig.Job1CronExpression));

    // Define the second job and tie it to the ExampleJob2 class
    var jobKey2 = new JobKey("ExampleJob2");
    q.AddJob<ExampleJob2>(opts => opts.WithIdentity(jobKey2));

    // Trigger the second job using a cron schedule from the configuration
    q.AddTrigger(opts => opts
        .ForJob(jobKey2)
        .WithIdentity("ExampleJob2-trigger")
        .StartNow()
        .WithCronSchedule(scheduleConfig.Job2CronExpression));
});

// Add Quartz.NET hosted service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
