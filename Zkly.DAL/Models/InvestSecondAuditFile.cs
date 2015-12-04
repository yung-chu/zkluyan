using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class InvestSecondAuditFile
    {
         [Key, ForeignKey("InvestSecondAuditInfo"), Column(Order = 0)]
        public long InvestSecondAuditId { get; set; }

        public virtual InvestSecondAuditInfo InvestSecondAuditInfo { get; set; }

        [Key, ForeignKey("UploadFileInfo"), Column(Order = 1)]
        public long UploadFileInfoesId { get; set; }

        public virtual UploadFileInfo UploadFileInfo { get; set; }

        //文件类型
        public long FileTypeId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
