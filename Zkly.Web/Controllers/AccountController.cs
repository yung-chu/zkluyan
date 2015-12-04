using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CaptchaMvc.HtmlHelpers;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Zkly.BLL.Account;
using Zkly.BLL.Repositories;
using Zkly.BLL.ViewModels;
using Zkly.Common.Log;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private InvestRepository _investRepository;
        private DataDictionaryReponsitory _dataDictionaryReponsitory;

        #region 构造
        public AccountController(InvestRepository investRepository, DataDictionaryReponsitory dataDictionaryReponsitory)
        {
            _investRepository = investRepository;
            _dataDictionaryReponsitory = dataDictionaryReponsitory;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        #endregion

        #region 登录
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //add bu zhuyong
            //修复解决用户过了锁定时间不能解锁的bug 
            var applicationUser = await UserManager.FindByNameAsync(model.UserName);
            if (applicationUser != null && applicationUser.LockoutEnabled && applicationUser.LockoutEndDateUtc < System.DateTime.Now)
            {
                await UserManager.SetLockoutEnabledAsync(applicationUser.Id, false);
            }

            //用户名,邮箱,手机登录（启用安全登录需验证邮箱和电话）
            // 若要在多次输入错误密码的情况下触发帐户锁定，请更改为 shouldLockout: true
            var result = await SignInManager.SignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, "用户名或密码输入错误,请重新输入");
                    return View(model);
            }
        }
        #endregion

        #region 登录：短信/邮件验证码
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                ViewBag.Message = "找不到用户ID";
                return View("Error");
            }

            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }

            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }

            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError(string.Empty, "代码无效。");
                    return View(model);
            }
        }
        #endregion

        #region 注册
        // GET: /Account/Register?role=Investor
        [AllowAnonymous]
        public ActionResult Register(EUserRole? role)
        {
            DataDictionary dataDictionaries = _dataDictionaryReponsitory.GetDataDictionary(1, Zkly.Common.Dictionary.Enums.DataDictionaryType.IsRegister.ToString());
            var model = new RegisterViewModel { Role = role.GetValueOrDefault(), DataDictionaries = dataDictionaries };
            return View(model);
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && model.Role != EUserRole.Administrator && this.IsCaptchaValid("您输入的验证码有误。"))
            {
                var user = new ApplicationUser { UserName = model.UserName, DisplayName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, model.Role);
                    if (result.Succeeded)
                    {
                        //  Comment the following line to prevent log in until the user is confirmed.
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "激活你的帐户");

                        //第二步--手机验证
                        return RedirectToAction(
                            "VerifyPhone",
                            new UserVerifyInfo()
                                {
                                    UserId = user.Id
                                });
                    }
                }

                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        #region 注册第二,三步

        #region 手机验证
        [AllowAnonymous]
        public ActionResult VerifyPhone(UserVerifyInfo model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return View("Error");
            }

            model.PhoneNumber = UserManager.GetPhoneNumber(model.UserId);
            model.UserId = model.UserId;

            return View(model);
        }

        // 生成令牌并发送该令牌
        [AllowAnonymous]
        public async Task<JsonResult> SendPhoneConfirmationToken(string userId)
        {
            bool resultInfo = false;

            if (string.IsNullOrEmpty(userId))
            {
                return new JsonResult() { Data = new { result = resultInfo, message = resultInfo ? "正在发送中...." : "发送失败，请重试" } };
            }

            try
            {
                var number = await UserManager.GetPhoneNumberAsync(userId);
                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, number);
                if (UserManager.SmsService != null)
                {
                    var message = new IdentityMessage
                    {
                        Destination = number,
                        Body = string.Format("欢迎注册，您的手机验证码为{0}，请勿泄露。", code)
                    };
                    await UserManager.SmsService.SendAsync(message);
                }

                resultInfo = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

            return Json(
                new { result = resultInfo, message = resultInfo ? "正在发送中...." : "发送失败，请重试" },
                JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<JsonResult> VerifyPhoneNumber(VerifyPhoneNumberInfo info)
        {
            var verifyResult = false;

            var number = await UserManager.GetPhoneNumberAsync(info.UserId);
            var result = await UserManager.ChangePhoneNumberAsync(info.UserId, number, info.Code);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(info.UserId);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                verifyResult = true;
            }

            return this.Json(new { result = verifyResult, JsonRequestBehavior.AllowGet });
        }

        public class VerifyPhoneNumberInfo
        {
            public string UserId { get; set; }

            public string Code { get; set; }
        }

        #endregion

        #region 页面跳转

        [AllowAnonymous]
        public ActionResult RegisterSuccess(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return View("Error");
            }

            var model = new UserRegisterSuccess
            {
                Url = PageJump(userId)
            };

            return View(model);
        }

        [AllowAnonymous]
        public string PageJump(string userId)
        {
            string jumpUrl = "/Home/Index";
            if (UserManager.IsInRole(userId, "Enterprise"))
            {
                //跳转到企业个人管理页面
                jumpUrl = "/Enterprise/Index";
            }
            else if (UserManager.IsInRole(userId, "Investor"))
            {
                //查询是否已经进行偏好设置
                InvestRepository invest = new InvestRepository();
                InvestorPreference i = invest.Preference(userId);
                if (i == null)
                {
                    //未进行了偏好设计
                    jumpUrl = "/Manage/invester-preference";
                }
            }

            return jumpUrl;
        }

        #endregion

        #endregion
        #region 注册前台验证
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckIsExistUserName(string userName)
        {
            var userModel = await UserManager.FindByNameAsync(userName);

            bool result = userModel == null;

            return Json(new { valid = result, message = "用户名已存在" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckIsExistPhone(string phoneNumber)
        {
            var userModel = await UserManager.FindByPhoneAsync(phoneNumber);

            bool result = userModel == null;

            return Json(new { valid = result, message = "手机号码已存在" });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> CheckIsExistEmail(string email)
        {
            var userModel = await UserManager.FindByEmailAsync(email);

            bool result = userModel == null;

            return Json(new { valid = result, message = "邮箱已存在" });
        }

        #endregion
        #endregion
        #region 忘记/重置密码
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        [Route("forgot-password")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveForgotPassword(ForgotPasswordViewModel model)
        {
            ModelState.Remove("Number");

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                if (user == null)
                {
                    // 请不要显示该用户不存在或者未经确认
                    return View("ForgotPasswordConfirmation");
                }

                // 启用帐户确认和密码重置，发送包含此链接的电子邮件
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { guidId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "重置密码", "请通过单击 <a href=\"" + callbackUrl + "\">此处</a>来重置你的密码");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View("ForgotPassword", model);
        }

        public ActionResult EnterPhoneNumber()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnterPhoneNumber(ForgotPasswordViewModel model)
        {
            ModelState.Remove("Email");
            var getUser = await UserManager.FindByPhoneAsync(model.Number);
            if (getUser == null)
            {
                ModelState.AddModelError(string.Empty, "该手机号不存在");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 生成令牌并发送该令牌
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(getUser.Id, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = string.Format("欢迎注册，您的手机验证码为{0}，请勿泄露。", code)
                };
                await UserManager.SmsService.SendAsync(message);
            }

            return RedirectToAction("ForgotPasswordVerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        [AllowAnonymous]
        [Route("forget-password-verify-phone")]
        public ActionResult ForgotPasswordVerifyPhoneNumber(string phoneNumber)
        {
            // 通过 SMS 提供程序发送短信以验证电话号码
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveForgotPasswordVerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            var getUser = await UserManager.FindByPhoneAsync(model.PhoneNumber);

            if (!ModelState.IsValid)
            {
                return View("ForgotPasswordVerifyPhoneNumber", model);
            }

            if (getUser == null)
            {
                return View("ForgotPasswordVerifyPhoneNumber", model);
            }

            var result = await UserManager.ChangePhoneNumberAsync(getUser.Id, model.PhoneNumber, model.Code);

            //重置密码
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPassword", "Account", new { guidId = getUser.Id, code = model.Code });
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ModelState.AddModelError(string.Empty, "验证码错误");
            return View("ForgotPasswordVerifyPhoneNumber", model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [Route("reset-password")]
        public ActionResult ResetPassword(string guidId, string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var user = await UserManager.FindByNameAsync(model.Email);
            //if (user == null) 
            //{
            //    // 请不要显示该用户不存在
            //    return RedirectToAction("ResetPasswordConfirmation", "Account");
            //}
            var result = await UserManager.ResetPasswordAsync(model.GuidId, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            AddErrors(result);
            return View("ResetPassword");
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
        #region 邮箱验证
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                ViewBag.Message = "无效的邮件激活链接！";
                return View("Error");
            }

            IdentityResult result;
            try
            {
                result = await UserManager.ConfirmEmailAsync(userId, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                ViewBag.Message = ioe.Message;
                return View("Error");
            }

            if (result.Succeeded)
            {
                return View();
            }

            // If we got this far, something failed.
            AddErrors(result);
            ViewBag.Message = "邮件激活失败，请重试！";
            return View("Error");
        }

        // GET: /Account/SendEmailConfirmationToken
        public async Task<ActionResult> SendEmailConfirmationToken()
        {
            await SendEmailConfirmationTokenAsync(User.Identity.GetUserId(), "邮箱验证");
            ViewBag.Message = "邮箱验证邮件已经发往你的邮箱，请注意查收！";
            return View("Info");
        }

        // 启用帐户确认和密码重置的，发送包含此链接的电子邮件
        private async Task<string> SendEmailConfirmationTokenAsync(string userID, string subject)
        {
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID, subject, "请点击 <a href=\"" + callbackUrl + "\">这里</a>来激活你的帐户");
            return callbackUrl;
        }
        #endregion

        #region ExternalLogin
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Profile");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion

        // POST: /Account/LogOff
        #region
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            var model = new ResultViewModel
            {
                ControllerName = "Home",
                ActionName = "Index"
            };

            return RedirectToAction("LogOffSuccess", model);
        }

        [AllowAnonymous]
        public ActionResult LogOffSuccess(ResultViewModel model)
        {
            ViewBag.Message = "注销成功";
            return View("Success_Info", model);
        }
        #endregion

        #region 帮助程序

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}