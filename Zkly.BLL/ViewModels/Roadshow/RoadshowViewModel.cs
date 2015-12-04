using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zkly.BLL.ViewModels
{
    public class RoadshowViewModel
    {
        public string Guid { get; set; }

        public long Id { get; set; }

        [Required(ErrorMessage = "视频名称不能为空")]
        public string VideoName { get; set; }

        [Required(ErrorMessage = "视频简介不能为空")]
        public string VideoDescrition { get; set; }

        public long VideoSize { get; set; }

        public long CoverFileId { get; set; }

        //资本路演 或者 业务路演
        public string Type { get; set; }

        public string RoadshowAddress { get; set; }

        public HttpPostedFileBase VideoFile { get; set; }

        public HttpPostedFileBase CoverFile { get; set; }

        public DateTime SubmitDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
