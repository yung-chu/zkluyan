using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class RoadshowUploadWatcher
    {
        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "文件夹长度不能大于100！")]
        public string Folder { get; set; }

        /// <summary>
        /// FileName是唯一的,可以作为关联主键
        /// </summary>
        [MaxLength(100, ErrorMessage = "文件名长度不能大于100！")]
        public string FileName { get; set; }

        /// <summary>
        /// 0 - 未同步到Vhall
        /// 1 - 正在同步到Vhall
        /// 2 - 已同步到Vhall
        /// </summary>
        public int SyncStatus { get; set; }

        [MaxLength(100, ErrorMessage = "服务器名长度不能大于100！")]
        public string ServerName { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
