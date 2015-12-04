using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common;
using Zkly.Common.Config;
using Zkly.Common.Dictionary;

namespace Zkly.DAL.Models
{
    public enum EActivityStatus
    {
        [Display(Name = "预约中")]
        Ordering0 = 0,

        [Display(Name = "预约中")]
        Ordering1 = 1,

        [Display(Name = "直播中")]
        Playing = 2,

        [Display(Name = "结束")]
        End = 3,

        [Display(Name = "录播已上线")]
        Recording = 8
    }

    public class CapitalRoadshow
    {
        [Key, ForeignKey("Invest")]
        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "公司名称长度不能大于100！")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "活动主题不能为空！")]
        [MaxLength(50, ErrorMessage = "活动主题长度不能大于50！")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "开始时间不能为空！")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "结束时间不能为空！")]
        public DateTime EndDate { get; set; }

        [MaxLength(30, ErrorMessage = "主持人长度不能大于30！")]
        public string Hoster { get; set; }

        [MaxLength(500, ErrorMessage = "描述长度不能大于4000！")]
        public string Description { get; set; }

        [RegularExpression(@"^[A-Za-z0-9]{6,20}$", ErrorMessage = "公共密码格式错误，6-20位英文字母、数字或组合")]
        [MaxLength(20, ErrorMessage = "公共密码长度不能大于20！")]
        public string PublicPassword { get; set; }

        [MaxLength(10, ErrorMessage = "活动ID长度不能大于10！")]
        public string WebinarId { get; set; }

        public EActivityStatus Status { get; set; }

        [NotMapped]
        public HttpPostedFileBase CoverFile { get; set; }

        public virtual Invest Invest { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        //代表是否修改封面
        [NotMapped]
        public int IsChange { get; set; }

        //文件Id
        public long FileId { get; set; }

        [DefaultValue(true)]
        public bool RecordState { get; set; }
    }
}
