using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common;

namespace Zkly.DAL.Models
{
    public class Roadshow
    {
        [Key, ForeignKey("Invest")]
        public long Id { get; set; }

        public int Priority { get; set; }

        [Required(ErrorMessage = "视频名不能为空！")]
        [MaxLength(100, ErrorMessage = "视频名长度不能大于100！")]
        public string VideoName { get; set; }

        [Required(ErrorMessage = "视频简介不能为空！")]
        [MaxLength(200, ErrorMessage = "视频简介长度不能大于200！")]
        public string VideoDescrition { get; set; }

        public long VideoSize { get; set; }

        [MaxLength(100, ErrorMessage = "视频文件名长度不能大于100！")]
        public string VideoFileName { get; set; }

        public long CoverFileId { get; set; }

        [MaxLength(100, ErrorMessage = "视频所在文件夹长度不能大于100！")]
        public string Folder { get; set; }

        [MaxLength(100, ErrorMessage = "Vhall视频地址长度不能大于100！")]
        public string VhallRoadshowAddress { get; set; }

        public bool IsOnShow { get; set; }

        public DateTime SubmitDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public virtual Invest Invest { get; set; }
    }
}
