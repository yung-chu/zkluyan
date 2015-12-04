using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class MetaIndex
    {
        public long Id { get; set; }

        [MaxLength(20, ErrorMessage = "索引文件长度不能大于20！")]
        public string IndexFile { get; set; }

        [MaxLength(20, ErrorMessage = "视频文件名称长度不能大于20！")]
        public string VideoName { get; set; }

        [MaxLength(20, ErrorMessage = "Vhall视频地址长度不能大于20！")]
        public string VhallShowAddress { get; set; }

        [MaxLength(20, ErrorMessage = "错误信息长度不能大于20！")]
        public string Error { get; set; }
    }
}
