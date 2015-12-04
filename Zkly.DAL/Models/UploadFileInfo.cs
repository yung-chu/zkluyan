using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class UploadFileInfo
    {
        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "文件路径长度不能大于100！")]
        public string FilePath { get; set; }

        [MaxLength(100, ErrorMessage = "文件名长度不能大于100！")]
        public string FileName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
