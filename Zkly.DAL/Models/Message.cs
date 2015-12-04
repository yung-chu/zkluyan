using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class Message
    {
        public long Id { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        [MaxLength(128)]
        public string From { get; set; }

        [MaxLength(128)]
        public string To { get; set; }

        [MaxLength(100, ErrorMessage = "主题长度不能大于100！")]
        public string Subject { get; set; }

        [MaxLength(500, ErrorMessage = "内容长度不能大于500！")]
        public string Body { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool State { get; set; } //已读，未读

        public virtual ApplicationUser User { get; set; }
    }
}
