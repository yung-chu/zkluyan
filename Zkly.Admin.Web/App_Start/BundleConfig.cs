namespace Zkly.Admin.Web
{
    using System.Web.Optimization;
    using Zkly.Admin.Web.Constants;

    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = false;
            AddCss(bundles);
            AddJavaScript(bundles);
        }

        private static void AddCss(BundleCollection bundles)
        {
            // Bootstrap - Twitter Bootstrap CSS (http://getbootstrap.com/).
            // Font Awesome - Icons using font (http://fortawesome.github.io/Font-Awesome/).
            // Site - Your custom site css.
            // Note: No CDN support has been added here. Most likely you will want to customize your copy of bootstrap.
            // Note: If you host any of your CSS on a seperate domain (Like a CDN), then be sure to fix an issue with respond.js which stops working for IE8.
            bundles.Add(new StyleBundle("~/Content/style/main").Include(
                "~/Content/bootstrap/site.css",
                "~/Content/fontawesome/site.css",
                "~/Content/Styles/Public/site.css"));

            bundles.Add(new StyleBundle("~/style/invest").Include(
                "~/Content/Invest/invest.css",
                "~/Content/Invest/first-audit.css"));

            bundles.Add(new StyleBundle("~/style/roadshow").Include(
                "~/Content/Roadshow/business-roadshow.css",
                "~/Content/Roadshow/capital-roadshow.css"));

            bundles.Add(new StyleBundle("~/style/loan").Include(
                "~/Content/Loan/loan.css"));

            bundles.Add(new StyleBundle("~/css/fileinput").Include(
        "~/Content/Styles/ThirdParty/fileinput.min.css"));

            bundles.Add(new StyleBundle("~/css/bootstrapValidatorCss").Include(
       "~/Content/Styles/Public/bootstrapValidator.css"));
        }

        /// <summary>
        /// Creates and adds JavaScript bundles to the bundle collection. 
        /// Content Delivery Network's (CDN) are used where available. 
        /// Note: MVC's built in
        /// <see cref="System.Web.Optimization.Bundle.CdnFallbackExpression"/> is not used as using inline
        /// scripts is not permitted under Content Security Policy (CSP) (See FilterConfig for more details).
        /// Instead, we create our own failover bundles. If a CDN is not reachable, the failover script
        /// loads the local bundles instead. The failover script is only a few lines of code and should have
        /// a minimal impact, although it does add an extra request (Two if the browser is IE8 or less).
        /// If you feel confident in the CDN availability and prefer better performance, you can delete these lines.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        private static void AddJavaScript(BundleCollection bundles)
        {
            AddPlugInJavaScript(bundles);
            AddBusineseJavaScript(bundles);
        }

        //第三方js
        private static void AddPlugInJavaScript(BundleCollection bundles)
        {
            // jQuery - The JavaScript helper API (http://jquery.com/).
            Bundle jqueryBundle = new ScriptBundle("~/js/jquery")
                .Include("~/Scripts/libraries/jquery-{version}.js")
                .Include("~/Scripts/libraries/jquery.plugins.js");
            bundles.Add(jqueryBundle);

            bundles.Add(new ScriptBundle("~/js/fileinput").Include(
                    "~/Scripts/fileinput.js"));

            // jQuery Validate - Client side JavaScript form validation (http://jqueryvalidation.org/).
            Bundle jqueryValidateBundle = new ScriptBundle("~/js/jqueryval")
                .Include("~/Scripts/libraries/jquery.validate*");
            bundles.Add(jqueryValidateBundle);

            // Microsoft jQuery Validate Unobtrusive - Validation using HTML data- attributes (http://stackoverflow.com/questions/11534910/what-is-jquery-unobtrusive-validation)
            Bundle jqueryValidateUnobtrusiveBundle = new ScriptBundle("~/js/jqueryvalunobtrusive")
                .Include("~/Scripts/libraries/jquery.validate*");
            bundles.Add(jqueryValidateUnobtrusiveBundle);

            Bundle jqueryJsonBundle = new ScriptBundle("~/js/jqueryjson")
                .Include("~/Scripts/libraries/jquery.json.js");
            bundles.Add(jqueryJsonBundle);

            // Modernizr - Allows you to check if a particular API is available in the browser (http://modernizr.com).
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            // Note: The current version of Modernizr does not support Content Security Policy (CSP) (See FilterConfig).
            // In addition the CDN version is old. So we are using a local copy which skips some CSP violating checks 
            // and returns true for them. This is REALLY bad and needs to be fixed soon. See here for details:
            // https://github.com/Modernizr/Modernizr/pull/1263 and http://stackoverflow.com/questions/26532234/modernizr-causes-content-security-policy-csp-violation-errors
            Bundle modernizrBundle = new ScriptBundle("~/js/modernizr")
                ////.Include("~/Scripts/modernizr-*");
                .Include("~/Scripts/libraries/modernizr-*");
            bundles.Add(modernizrBundle);

            // Bootstrap - Twitter Bootstrap JavaScript (http://getbootstrap.com/).
            Bundle bootstrapBundle = new ScriptBundle("~/js/bootstrap")
                .Include("~/Scripts/libraries/bootstrap.js");
            bundles.Add(bootstrapBundle);

            // Respond.js - A fast & lightweight polyfill for min/max-width CSS3 Media Queries (https://github.com/scottjehl/Respond). 
            // Note: that the CDN version is a little behind the latest 1.4.2.
            Bundle respondBundle = new ScriptBundle("~/js/respond")
                .Include("~/Scripts/libraries/respond.js");
            bundles.Add(respondBundle);

            // Failover Core - If the CDN's fail then these scripts load local copies of the same scripts.
            Bundle failoverCoreBundle = new ScriptBundle("~/js/failovercore")
                .Include("~/Scripts/Failover/jquery.js")
                .Include("~/Scripts/Failover/jqueryval.js")
                .Include("~/Scripts/Failover/jqueryvalunobtrusive.js")
                .Include("~/Scripts/Failover/bootstrap.js");
            bundles.Add(failoverCoreBundle);

            // Failover Respond - If the Respond.js CDN fails then this script loads a local copy. 
            // Note: This script is only used if the browser is running IE8 or less.
            bundles.Add(new ScriptBundle("~/js/failoverrespond")
                .Include("~/Scripts/Failover/respond.js"));
        }

        //业务js
        private static void AddBusineseJavaScript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/investaudit")
                .Include("~/Scripts/Invest/invest-audit.js"));

            bundles.Add(new ScriptBundle("~/js/bizroadshow")
                .Include("~/Scripts/RoadShow/business-roadshow.js"));

            bundles.Add(new ScriptBundle("~/js/capital-roadshow")
                .Include("~/Scripts/libraries/bootstrap-datepicker.js")
                .Include("~/Scripts/RoadShow/capital-roadshow.js"));

            bundles.Add(new ScriptBundle("~/js/capital-roadshow-new")
               .Include("~/Scripts/libraries/bootstrap-datepicker.js")
               .Include("~/Scripts/RoadShow/capital-roadshow-new.js"));

            bundles.Add(new ScriptBundle("~/js/placeTempFolder")
                .Include("~/Scripts/TempFolder/placeTempFolder.js"));

            bundles.Add(new ScriptBundle("~/js/deleteTempFolder")
              .Include("~/Scripts/TempFolder/deleteTempFolder.js"));

            bundles.Add(new ScriptBundle("~/js/profile")
                .Include("~/Scripts/Profile/profile.js"));

            bundles.Add(new ScriptBundle("~/js/user-management")
                .Include("~/Scripts/Management/user-management.js"));

            bundles.Add(new ScriptBundle("~/js/loan")
                .Include("~/Scripts/Loan/loan.js"));

            bundles.Add(new ScriptBundle("~/js/DropDownSel")
             .Include("~/Scripts/Management/DropDownSel.js"));

            bundles.Add(new ScriptBundle("~/js/agencyCommission")
                .Include("~/Scripts/Commission/agencyCommission.js"));

            bundles.Add(new ScriptBundle("~/js/bootstrapValidator").Include(
            "~/Scripts/libraries/bootstrapValidator.js"));

            bundles.Add(new ScriptBundle("~/js/user")
            .Include("~/Scripts/Management/user.js"));

            bundles.Add(new ScriptBundle("~/js/invest_Verification")
            .Include("~/Scripts/Management/invest_Verification.js"));

            bundles.Add(new ScriptBundle("~/js/loan_Verification")
            .Include("~/Scripts/Management/loan_Verification.js"));

            bundles.Add(new ScriptBundle("~/js/Verification")
            .Include("~/Scripts/Sms/Verification.js"));

            bundles.Add(new ScriptBundle("~/js/AddPhoneNumber")
            .Include("~/Scripts/profile/AddPhoneNumber.js"));

            bundles.Add(new ScriptBundle("~/js/ChangePassword")
            .Include("~/Scripts/profile/ChangePassword.js"));

            bundles.Add(new ScriptBundle("~/js/SetPassword")
            .Include("~/Scripts/profile/SetPassword.js"));

            bundles.Add(new ScriptBundle("~/js/VerifyPhoneNumber")
            .Include("~/Scripts/profile/VerifyPhoneNumber.js"));

            bundles.Add(new ScriptBundle("~/js/capital-roadshow-Verification")
            .Include("~/Scripts/RoadShow/capital-roadshow-Verification.js"));
        }
    }
}
