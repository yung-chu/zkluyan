using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Zkly.Common.Config;
using Zkly.Common.Dictionary;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public interface IRoadshowWatcherRepository
    {
        List<RoadshowUploadWatcher> GetUnuploadedRoadshows(int maxCount);

        void ResetStatus();

        bool SetProcessingStatus(RoadshowUploadWatcher entity);

        bool SetFinishStatus(RoadshowUploadWatcher entity);
    }

    public class RoadshowWatcherRepository : IRoadshowWatcherRepository
    {
        public List<RoadshowUploadWatcher> GetUnuploadedRoadshows(int maxCount)
        {
            using (var db = new UserDbContext())
            {
                return db.RoadshowUploadWatchers.Where(r => r.SyncStatus == 0 && r.ServerName == AppSettings.MachineName)
                        .Take(maxCount)
                        .ToList();
            }
        }

        public void ResetStatus()
        {
            using (var db = new UserDbContext())
            {
                var list = db.RoadshowUploadWatchers.Where(r => r.SyncStatus == (int)Enums.SyncStatus.OnSync && r.ServerName == AppSettings.MachineName).ToList();
                list.ForEach(r =>
                {
                    r.SyncStatus = (int)Enums.SyncStatus.UnSync;
                    r.UpdateTime = DateTime.Now;
                    db.Entry(r).State = EntityState.Modified;
                });
                db.SaveChanges();
            }
        }

        public bool SetProcessingStatus(RoadshowUploadWatcher entity)
        {
            using (var db = new UserDbContext())
            {
                entity.SyncStatus = (int)Enums.SyncStatus.OnSync;
                entity.UpdateTime = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                return db.SaveChanges() == 1;
            }
        }

        public bool SetFinishStatus(RoadshowUploadWatcher entity)
        {
            using (var db = new UserDbContext())
            {
                entity.SyncStatus = (int)Enums.SyncStatus.OkSync;
                entity.UpdateTime = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                return db.SaveChanges() == 1;
            }
        }

        public bool SetErrorStatus(RoadshowUploadWatcher entity)
        {
            using (var db = new UserDbContext())
            {
                entity.SyncStatus = 3;
                entity.UpdateTime = DateTime.Now;
                db.Entry(entity).State = EntityState.Modified;
                return db.SaveChanges() == 1;
            }
        }
    }
}
