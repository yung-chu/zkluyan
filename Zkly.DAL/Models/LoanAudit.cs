using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class LoanAudit
    {
        public long Id { get; set; }

        public EInvestAuditStage Stage { get; set; }

        public EInvestAuditStatus Status { get; set; }

        public long? AuditQuota { get; set; }

        [MaxLength(500, ErrorMessage = "原因长度不能大于500！")]
        public string FailReason { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
