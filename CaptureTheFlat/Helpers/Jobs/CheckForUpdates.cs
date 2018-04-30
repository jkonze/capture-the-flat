using System;
using FluentScheduler;
using Microsoft.AspNetCore.Hosting.Internal;

namespace CaptureTheFlat.Helpers.Jobs
{
    public class CheckForUpdates : IJob
    {
        private readonly object _lock = new object();

        private bool _shuttingDown;

        public CheckForUpdates()
        {
            // Register this job with the hosting environment.
            // Allows for a more graceful stop of the job, in the case of IIS shutting down.
        }

        public void Execute()
        {
            try
            {
                lock (_lock)
                {
                    if (_shuttingDown)
                        return;
                    
                    // Do work, son!
                    Console.Write("Nice");
                }
            }
            finally
            {
                // Always unregister the job when done.
            }
        }

        public void Stop(bool immediate)
        {
            // Locking here will wait for the lock in Execute to be released until this code can continue.
            lock (_lock)
            {
                _shuttingDown = true;
            }

        }
    }
}
