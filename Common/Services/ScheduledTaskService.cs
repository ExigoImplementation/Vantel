using System;
using System.Threading.Tasks;

namespace Common.Services
{
    public class ScheduledTask
    {
        public static void Start(Action task)
        {
            Start(3600, task);
        }
        public static void Start(int interval, Action task)
        {
            Task.Run(async delegate
            {
                while (true)
                {
                    task();
                    await Task.Delay(interval * 1000);
                }
            });
        }
    }
}
