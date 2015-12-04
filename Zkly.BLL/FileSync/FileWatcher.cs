using System;
using System.Threading;
using Zkly.Common.Config;

namespace Zkly.BLL.FileSync
{
    public class FileWatcher
    {
        private static readonly RoadshowSynchronizer Syncer = new RoadshowSynchronizer();
        private static Timer timer = null;

        public static void Register()
        {
            Syncer.Init();

            timer = new Timer(
                (state) =>
                {
                    Syncer.Sync();
                    timer.Change(AppSettings.FileWatcherInterval, Timeout.Infinite);
                },
            null,
            AppSettings.FileWatcherInterval,
            Timeout.Infinite);

            //Timer.Interval = AppSettings.FileWatcherInterval;
            //Timer.Enabled = true;
            //Timer.Elapsed += OnProcess;
        }

        //public static void OnProcess(object sender, ElapsedEventArgs e)
        //{
        //    Timer.Stop();
        //    try
        //    {
        //        Syncer.Sync();
        //        Timer.Start();
        //    }
        //    catch (Exception)
        //    {
        //        Timer.Start();
        //    }
        //}
    }
}