using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.Linq;
using System.Web;

using Zkly.Common.Config;
using Zkly.Common.Dictionary;
using Zkly.Common.Extension;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public interface IInvestAgreementRepository: IRepositoryBase
    {
        Invest GetInvest(long investId);

        InvestAgreement GetInvestAgreement(long investId);

        IList<AgencyCommission> GetCashOptions();

        IList<AgencyCommission> GetStockOptions();

        IList<AgencyCommission> GetCombineOptions();

        string SaveAgreement(long investId, short lockMonth, int agencyCommissionId, HttpPostedFileBase investAgreement);

        string UpdateAgreement(long investId, HttpPostedFileBase investAgreement);
    }

    public class InvestAgreementRepository : RepositoryBase<UserDbContext>, IInvestAgreementRepository
    {
        public InvestAgreementRepository()
            : base(throwExceptions: true, useTransactions: false) //isolation not supported in LocalDB
        {
        }

        public Invest GetInvest(long investId)
        {
            return FirstOrDefault<Invest>(invest => invest.Id == investId);
        }

        public InvestAgreement GetInvestAgreement(long investId)
        {
            return FirstOrDefault<InvestAgreement>(agreement => agreement.Id == investId, agreement => agreement.AgencyCommission, agreement => agreement.AgreementFile);
        }

        public IList<AgencyCommission> GetCashOptions()
        {
            return Find<AgencyCommission>(commission => commission.CashPercent > 0f && commission.StockPercent == 0f).ToList();
        }

        public IList<AgencyCommission> GetStockOptions()
        {
            return Find<AgencyCommission>(commission => commission.CashPercent == 0f && commission.StockPercent > 0f).ToList();
        }

        public IList<AgencyCommission> GetCombineOptions()
        {
            return Find<AgencyCommission>(commission => commission.CashPercent > 0f && commission.StockPercent > 0f).ToList();
        }

        public string SaveAgreement(long investId, short lockMonth, int agencyCommissionId, HttpPostedFileBase investAgreement)
        {
            try
            {
                long agreementFileId = 0;
                InvestAgreement investAgreementEnity = GetInvestAgreement(investId);
                Invest invest = GetInvest(investId);

                if (investAgreement.IsNullHttpPostedFile())
                {
                    UploadFileInfo uploadFileInfo = new UploadFileInfoRepository().GetFileInfo(
                        investAgreement,
                        (int)Enums.FolderPath.InvestAgreement);
                    agreementFileId = uploadFileInfo.Id;
                }
                else
                {
                    if (investAgreementEnity == null)
                    {
                        return "请上传协议";
                    }
                    else
                    {
                        agreementFileId = investAgreementEnity.AgreementFileId.Value;
                    }
                }

                var entity = new InvestAgreement
                                 {
                                     Id = investId,
                                     LockMonth = lockMonth,
                                     AgencyCommissionId = agencyCommissionId,
                                     AgreementFileId = agreementFileId
                                 };

                if (investAgreementEnity != null)
                {
                    using (UserDbContext db = new UserDbContext())
                    {
                        db.Entry(entity).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    invest.UpdateTime = System.DateTime.Now;
                }
                else
                {
                    Add(entity);
                }

               //更改Invest状态
                invest.Stage = EInvestAuditStage.Agreement;
                invest.Status = EInvestAuditStatus.Submited;
                invest.Reason = "提交协议";
                
                Update(invest);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        //更新上传的文件id
        public string UpdateAgreement(long investId, HttpPostedFileBase investAgreement)
        {
            InvestAgreement investAgreementEntity = GetInvestAgreement(investId);
            if (investAgreementEntity==null)
            {
                return "请先下载协议";
            }

            UploadFileInfo uploadFileInfo = new UploadFileInfoRepository().GetFileInfo(
                investAgreement,
                (int)Enums.FolderPath.InvestAgreement);

            if (uploadFileInfo.Id == 0)
            {
                return "文件上传失败";
            }

            using (UserDbContext db = new UserDbContext())
            {
                investAgreementEntity.AgreementFileId = uploadFileInfo.Id;
                db.Entry(investAgreementEntity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return string.Empty;
        }
    }
}
