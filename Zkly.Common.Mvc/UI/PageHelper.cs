using System;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Zkly.Common.Mvc.UI
{
    public static class PageHelper
    {
        public static MvcHtmlString DisplayPagination<T>(this HtmlHelper<IPagedList<T>> htmlHelper, Func<int, string> generatePageUrl)
        {
            var options = new PagedListRenderOptions()
            {
                LinkToFirstPageFormat = "首页",
                LinkToNextPageFormat = "下一页",
                LinkToPreviousPageFormat = "上一页",
                LinkToLastPageFormat = "末页",
                DisplayItemSliceAndTotal = true,
                ItemSliceAndTotalFormat = "共{2}条，显示{0}-{1}条",
                DisplayPageCountAndCurrentLocation = false,
                PageCountAndCurrentLocationFormat = "页数{0}/{1}",
                MaximumPageNumbersToDisplay = 10
            };

            return htmlHelper.PagedListPager(htmlHelper.ViewData.Model, generatePageUrl, options);
        }
    }
}
