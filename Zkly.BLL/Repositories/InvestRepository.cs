using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.Linq;
using Autofac.Core;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class InvestRepository : RepositoryBase<UserDbContext>
    {
        public bool AddInvest(Invest invest, string operatorId = null)
        {
            if (!Add(invest))
            {
                return false;
            }

            var history = new InvestHistory
                              {
                                  AuditQuota = invest.AuditQuota,
                                  Remark = invest.Reason,
                                  Stage = invest.Stage,
                                  Status = invest.Status,
                                  UserId = operatorId ?? invest.UserId,
                                  InvestId = invest.Id,
                                  CreateTime = invest.ApplyTime
                              };

            return Add(history);
        }

        public bool UpdateInvest(Invest invest, string adminId = null)
        {
            invest.UpdateTime = DateTime.Now;
            var history = new InvestHistory
            {
                AuditQuota = invest.AuditQuota,
                Remark = invest.Reason,
                Stage = invest.Stage,
                Status = invest.Status,
                UserId = adminId ?? invest.UserId,
                InvestId = invest.Id,
                CreateTime = invest.UpdateTime
            };

            return Update(invest) && Add(history);
        }

        //二审提交的时相关表的操作
        public string UpdateInvestInfoAndOperateInvestSecondAuditFile(Invest invest, List<InvestSecondAuditFile> listInvestSecondAuditFiles)
        {
            string result = string.Empty;

            using (UserDbContext db = new UserDbContext())
            {
                invest.UpdateTime = DateTime.Now;
                var history = new InvestHistory
                {
                    AuditQuota = invest.AuditQuota,
                    Remark = invest.Reason,
                    Stage = invest.Stage,
                    Status = invest.Status,
                    UserId = invest.UserId,
                    InvestId = invest.Id,
                    CreateTime = invest.UpdateTime
                };

                invest.SecondAuditInfo.Id = invest.Id;
                db.InvestAuditHistories.Add(history);

                //二审文件上传记录表操作
                foreach (var item in listInvestSecondAuditFiles)
                {
                    item.InvestSecondAuditId = invest.Id;
                }

                result = new InvestSecondAuditFileReponsitory().AddOrUpdateInvestSecondAuditFile(listInvestSecondAuditFiles, db);

                //错误
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                //更新invest状态为二审已提交
                db.Entry(invest).State = EntityState.Modified;

                //add or update
                if (db.SecondAuditInfos.FirstOrDefault(p => p.Id == invest.Id) != null)
                {
                    db.Entry(invest.SecondAuditInfo).State = EntityState.Modified;
                }
                else
                {
                    db.SecondAuditInfos.Add(invest.SecondAuditInfo);
                }

                if (db.SaveChanges() == 0)
                {
                    return "二审相关数据保存失败";
                }
            }

            return result;
        }

        public IList<Invest> GetInvestsByUserId(string userId)
        {
            return Find<Invest>(x => x.UserId == userId, i => i.FirstAuditInfo).ToList();
        }

        public Invest GetInvestById(long? id)
        {
            return FirstOrDefault<Invest>(m => m.Id == id, m => m.FirstAuditInfo, m => m.SecondAuditInfo, m => m.CapitalRoadshow, m => m.Agreement, m => m.Agreement.AgencyCommission, m => m.Agreement.AgreementFile);
        }

        public IList<InvestHistory> GetInvestHistoryById(long investId)
        {
            return Find<InvestHistory>(h => h.InvestId == investId, h => h.User).OrderBy(h => h.CreateTime).ToList();
        }

        public List<Invest> GetInvestsByStage(EInvestAuditStage stage)
        {
            return Find<Invest>(i => i.Stage == stage, i => i.FirstAuditInfo).ToList();
        }

        public IQueryable<Invest> GetInvests(EInvestAuditStage stage, EInvestAuditStatus status)
        {
            return Find<Invest>(i => i.Stage == stage && i.Status == status, i => i.FirstAuditInfo).OrderBy(i => i.UpdateTime);
        }

        public List<Invest> GetWaitingInvest(EInvestAuditStage stage)
        {
            var invests = this.GetInvestsByStage(stage);

            return invests.Where(i => i.IsAuditable()).ToList();
        }

        public List<Invest> GetImmediatelyInvests(EInvestAuditStage stage)
        {
            var invests = this.GetInvestsByStage(stage);

            var tempInvests = this.GetTemporaryInvests(stage);

            if (tempInvests.Count == 0)
            {
                return invests.Where(i => i.IsAuditable()).ToList();
            }

            return invests.Where(i => (NotInTemporarilyInvests(i, tempInvests) && i.IsAuditable())).ToList();
        }

        //todo: get temp folder by userid
        public List<Invest> GetTemporaryInvests(EInvestAuditStage stage)
        {
            //todo: dispose db
            var rp = new InvestTempFolderRepository();
            var list = rp.ListInvestTempFolder(stage);
            return list.Where(o => o.Invest.IsAuditable()).Select(o => o.Invest).ToList();
        }

        public InvestorPreference Preference(string userid)
        {
            return FirstOrDefault<InvestorPreference>(p => p.UserId == userid);
        }

        public IList<Invest> GetOnRoadshowInvest()
        {
            return Find<Invest>(i => i.Stage == EInvestAuditStage.Roadshow, i => i.FirstAuditInfo).ToList();
        }

        public IList<CapitalRoadshow> GetCapitalRoadshowByUserId(string userId)
        {
            var invests = Find<Invest>(i => i.UserId == userId);

            return invests.Where(o => o.CapitalRoadshow != null && o.CapitalRoadshow.RecordState).Select(o => o.CapitalRoadshow).ToList();
        }

        public List<Roadshow> GetBusinessRoadshowByUserId(string userId)
        {
            var invests = Find<Invest>(i => i.UserId == userId);

            return invests.Select(o => o.Roadshow).ToList();
        }

        private bool NotInTemporarilyInvests(Invest invest, List<Invest> temps)
        {
            var result = true;
            foreach (var t in temps)
            {
                if (t.Id == invest.Id)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        public List<Invest> GetHomePageInvestTop4(EInvestAuditStage stage)
        {
            var invests = this.GetInvestsByStage(stage);
            return invests.OrderByDescending(p => p.ApplyTime).Take(4).ToList();
        }

        public List<Invest> GetHomePageInvests(EInvestAuditStage stage)
        {
            var invests = this.GetInvestsByStage(stage);
            return invests.OrderByDescending(p => p.ApplyTime).ToList();
        }
    }
}
