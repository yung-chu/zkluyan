using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.DAL.Models
{
    public class DataDictionary
    {
        //主键Id
        [Key, Column(Order = 0)]
        public long DataDictionaryId { get; set; }

        //引用对象
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public string DataDictionaryType { get; set; }

        //文件名称
        [MaxLength(50)]
        public string DataDictionaryName { get; set; }

        //编码值
        [MaxLength(50)]
        public string LevelCode { get; set; }

        //标记码
        [MaxLength(50)]
        public string Symbol { get; set; }

        //父类Id
        public int ParentId { get; set; }

        //显示顺序
        public int DisplayOrder { get; set; }
    }
}
