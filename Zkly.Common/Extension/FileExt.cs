using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Zkly.Common.Extension
{
    public static class FileExt
    {
        public static string GetFileSuffix(this HttpPostedFileBase source)
        {
            if (source == null)
            {
                return string.Empty;
            }

            return Path.GetExtension(source.FileName); 
        }

        public static bool IsNullHttpPostedFile(this HttpPostedFileBase httpPostedFileBase)
        {
            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength > 0)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        //超过多少兆
        public static bool IsOverlimitedCapacity(this HttpPostedFileBase httpPostedFileBase, int capacity)
        {
            if (IsNullHttpPostedFile(httpPostedFileBase)&&Convert.ToDouble(httpPostedFileBase.ContentLength)/ 1024d / 1024d > capacity)
            {
               return true;
            }

            return false;
        }
    }
}
