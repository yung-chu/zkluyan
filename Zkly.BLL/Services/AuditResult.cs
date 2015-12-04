using Zkly.DAL.Models;

namespace Zkly.BLL.Services
{
    public class AuditResult
    {
        public long InvestId { get; set; }

        public EInvestAuditStage Stage { get; set; }

        public bool IsPass { get; set; }

        public int? AuditQuota { get; set; }

        public string Reason { get; set; }

        public string FilterType { get; set; }
    }
}
