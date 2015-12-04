using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Zkly.Common.Extension
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory MyTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

        public static TResult RunSync<TResult>(this Func<Task<TResult>> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;
            return System.Threading.Tasks.TaskExtensions.Unwrap<TResult>(AsyncHelper.MyTaskFactory.StartNew<Task<TResult>>((Func<Task<TResult>>)(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }))).GetAwaiter().GetResult();
        }

        public static void RunSync(this Func<Task> func)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;
            System.Threading.Tasks.TaskExtensions.Unwrap(AsyncHelper.MyTaskFactory.StartNew<Task>((Func<Task>)(() =>
            {
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
                return func();
            }))).GetAwaiter().GetResult();
        }
    }
}
