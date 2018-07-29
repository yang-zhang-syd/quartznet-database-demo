using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Impl;

namespace quartznet_database_demo
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true);
            var config = builder.Build();

            Console.WriteLine("Initializing scheduler!");

            var properties = Utils.ParseQuartzConfig(config);
            var scheduler = new StdSchedulerFactory(properties).GetScheduler().Result;

            var helloJob = await scheduler.GetJobDetail(new JobKey("HelloJob"));
            if (helloJob == null)
            {
                Console.WriteLine("No HelloJob found in datastore. Creating a new Job record!");

                var helloJobCron = config["CronSchedule:hello.job"];
                helloJob = JobBuilder.Create<HelloJob>()
                    .WithIdentity("HelloJob")
                    .StoreDurably(true)
                    .Build();
                var trigger = TriggerBuilder.Create()
                    .WithIdentity("HelloJobTrigger")
                    .WithCronSchedule(helloJobCron, x => x.WithMisfireHandlingInstructionFireAndProceed())
                    .Build();
                await scheduler.ScheduleJob(helloJob, trigger);
            }
            else
            {
                Console.WriteLine("Read HelloJob from datastore!");
            }

            await scheduler.Start();

            Console.ReadLine();
        }
    }
}
