using System.Web;
using System.Web.Optimization;

namespace Zkly.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterPlugInBundles(bundles);
            RegisterBusinessBundles(bundles);
            RegisterCssBundles(bundles);
        }

        //js,第三方library
        public static void RegisterPlugInBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
           "~/Scripts/libraries/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/jqueryval").Include(
                        "~/Scripts/libraries/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/js/jqueryjson").Include(
                        "~/Scripts/libraries/jquery.json.js"));

            bundles.Add(new ScriptBundle("~/js/jquery-form").Include(
                        "~/Scripts/libraries/jquery.form.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/js/modernizr").Include(
                        "~/Scripts/libraries/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                      "~/Scripts/libraries/bootstrap.js",
                      "~/Scripts/libraries/respond.js"));

            bundles.Add(new ScriptBundle("~/js/datepicker")
                .Include("~/Scripts/libraries/bootstrap-datepicker.js"));

            bundles.Add(new ScriptBundle("~/js/bootstrapValidator").Include(
                "~/Scripts/libraries/bootstrapValidator.js"));

            bundles.Add(new ScriptBundle("~/js/fileinput").Include(
                     "~/Scripts/libraries/fileinput.js"));
        }

        //js,业务逻辑
        public static void RegisterBusinessBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/area")
                .Include("~/Scripts/Common/area.js")
                .Include("~/Scripts/Common/areaChangeEvent.js"));

            bundles.Add(new ScriptBundle("~/js/common").Include(
                      "~/Scripts/Common/common.js"));

            bundles.Add(new ScriptBundle("~/js/loan").Include(
                      "~/Scripts/Enterprise/loan.js"));

            bundles.Add(new ScriptBundle("~/js/roadshow-order").Include(
                      "~/Scripts/Roadshow/business-roadshow-show-order.js"));

            bundles.Add(new ScriptBundle("~/js/business-roadshow-manager").Include(
                      "~/Scripts/Roadshow/business-roadshow-manager.js"));

            bundles.Add(new ScriptBundle("~/js/capital-roadshow-manager").Include(
                      "~/Scripts/Roadshow/capital-roadshow-manager.js"));

            bundles.Add(new ScriptBundle("~/js/auditmanager").Include(
                      "~/Scripts/audit-status-manager.js"));

            bundles.Add(new ScriptBundle("~/js/upload").Include(
                      "~/Scripts/Roadshow/upload.js"));

            bundles.Add(new ScriptBundle("~/js/alertmessage").Include(
                       "~/Scripts/Common/alert-message.js"));

            bundles.Add(new ScriptBundle("~/js/contactService").Include(
                      "~/Scripts/Common/contactService.js"));

            bundles.Add(new ScriptBundle("~/js/messageCenter").Include(
                "~/Scripts/Enterprise/messageCenter.js"));

            bundles.Add(new ScriptBundle("~/js/registerForm").Include(
                "~/Scripts/Account/registerForm.js"));

            bundles.Add(new ScriptBundle("~/js/verifyPhone").Include(
                "~/Scripts/Account/verifyPhone.js"));

            bundles.Add(new ScriptBundle("~/js/logonVerify").Include(
                "~/Scripts/Account/logonVerify.js"));

            bundles.Add(new ScriptBundle("~/js/enterprise-invest")
                .Include("~/Scripts/libraries/bootstrap-datepicker.js")
                .Include("~/Scripts/Invest/invest.js"));
   
            bundles.Add(new ScriptBundle("~/js/first-audit")
                .Include("~/Scripts/Invest/first-audit.js"));

            bundles.Add(new ScriptBundle("~/js/second-audit")
               .Include("~/Scripts/Invest/second-audit.js"));

            bundles.Add(new ScriptBundle("~/js/setTimeout").Include(
             "~/Scripts/Common/setTimeout.js"));

            bundles.Add(new ScriptBundle("~/js/helpPageHover").Include(
                "~/Scripts/HelpPage/helpPageHover.js"));

            bundles.Add(new ScriptBundle("~/js/move").Include(
                "~/Scripts/Home/move.js"));

            bundles.Add(new ScriptBundle("~/js/BusinessLicenseImgChange").Include(
                "~/Scripts/Manage/BusinessLicenseImgChange.js"));

            bundles.Add(new ScriptBundle("~/js/navigationScroll").Include(
                "~/Scripts/HelpPage/navigationScroll.js"));

            bundles.Add(new ScriptBundle("~/js/FileUploadForSecondAuditInfo").Include(
                "~/Scripts/File/FileUploadForSecondAuditInfo.js"));

            bundles.Add(new ScriptBundle("~/js/invest-agreement").Include(
              "~/Scripts/Invest/invest-agreement.js"));
        }

        public static void RegisterCssBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css/css").Include(
                      "~/Content/bootstrap/Site.css",
                      "~/Content/Styles/ThirdParty/datepicker.css",
                      "~/Content/Styles/Public/site.css",
                      "~/Content/Styles/Other/QQAndWechat.css"));

            bundles.Add(new StyleBundle("~/css/roadshow-item").Include(
             "~/Content/Styles/Roadshow/roadshow-item.css"));

            bundles.Add(new StyleBundle("~/css/enterprise").Include(
             "~/Content/Styles/Enterprise/enterprise.css"));

            bundles.Add(new StyleBundle("~/css/business").Include(
             "~/Content/Styles/Roadshow/business.css"));

            bundles.Add(new StyleBundle("~/css/message_Center").Include(
          "~/Content/Styles/Information/messageCenter.css"));

            bundles.Add(new StyleBundle("~/css/fileinput").Include(
         "~/Content/Styles/ThirdParty/fileinput.css"));

            bundles.Add(new StyleBundle("~/css/invest").Include(
         "~/Content/Styles/Investment/invest.css"));

            bundles.Add(new StyleBundle("~/css/bootstrapValidatorCss").Include(
         "~/Content/Styles/Public/bootstrapValidator.css"));

            bundles.Add(new StyleBundle("~/css/HelpPage").Include(
            "~/Content/Styles/Other/HelpPage.css"));

            bundles.Add(new StyleBundle("~/css/AccounSetup").Include(
            "~/Content/Styles/User/AccounSetup.css"));

            bundles.Add(new StyleBundle("~/css/Login").Include(
            "~/Content/Styles/User/Login.css"));

            bundles.Add(new StyleBundle("~/css/Register").Include(
           "~/Content/Styles/User/Register.css"));

            bundles.Add(new StyleBundle("~/css/PagedList").Include(
           "~/Content/Styles/ThirdParty/PagedList.css"));

            bundles.Add(new StyleBundle("~/css/ProgressBar").Include(
           "~/Content/Styles/Public/ProgressBar.css"));

            bundles.Add(new StyleBundle("~/css/InvestorPreference").Include(
           "~/Content/Styles/Investment/InvestorPreference.css"));

            bundles.Add(new StyleBundle("~/css/News").Include(
          "~/Content/Styles/Other/News.css"));

            bundles.Add(new StyleBundle("~/css/TestAndVerify").Include(
          "~/Content/Styles/Public/TestAndVerify.css"));

            bundles.Add(new StyleBundle("~/css/common").Include(
          "~/Content/Common/common.css"));

            bundles.Add(new StyleBundle("~/css/loan").Include(
        "~/Content/Styles/Loan/loan.css"));
        }
    }
}
