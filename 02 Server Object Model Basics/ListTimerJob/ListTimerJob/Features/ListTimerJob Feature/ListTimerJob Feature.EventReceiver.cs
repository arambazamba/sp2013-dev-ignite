using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace ListTimerJob.Features.ListTimerJob_Feature
{

    [Guid("defdc571-ffd9-46f1-b529-9a5002aafd83")]
    public class ListTimerJob_FeatureEventReceiver : SPFeatureReceiver
    {
        const string List_JOB_NAME = "List Timer Job";

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            DeleteExistingJob(properties);
            
            // install the job
            ListTimerJob listLoggerJob = new ListTimerJob(List_JOB_NAME, (properties.Feature.Parent as SPSite).WebApplication);
            SPMinuteSchedule schedule = new SPMinuteSchedule { BeginSecond = 0, EndSecond = 59, Interval = 5 };
            listLoggerJob.Schedule = schedule;
            listLoggerJob.Update();

        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            DeleteExistingJob(properties);
        }

        private static void DeleteExistingJob(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == List_JOB_NAME)
                {
                    job.Delete();
                }
            }
        }
    }
}
