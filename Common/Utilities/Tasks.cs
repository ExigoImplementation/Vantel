using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Perform one or more tasks asyncronously.
        /// </summary>
        /// <param name="actions">The action(s) to perform</param>
        public static void RunAsyncTasks(params Action[] actions)
        {
            var tasks = new List<Task>();

            foreach (var action in actions)
            {
                tasks.Add(Task.Factory.StartNew(action));
            }

            Task.WaitAll(tasks.ToArray());
            tasks.Clear();
        }
    }
}