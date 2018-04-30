using CaptureTheFlat.Helpers.Jobs;
using FluentScheduler;

namespace CaptureTheFlat.Helpers
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            Schedule<CheckForUpdates>().ToRunEvery(1).Minutes();
        }
    }
}