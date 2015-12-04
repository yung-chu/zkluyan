using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Zkly.BLL.Account;
using Zkly.BLL.ViewModels;

namespace Zkly.Web.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "已更改你的密码。"
                : message == ManageMessageId.SetPasswordSuccess ? "已设置你的密码。"
                : message == ManageMessageId.SetTwoFactorSuccess ? "已设置你的安全登录方式。"
                : message == ManageMessageId.Error ? "出现错误。"
                : message == ManageMessageId.AddPhoneSuccess ? "已添加你的电话号码。"
                : message == ManageMessageId.RemovePhoneSuccess ? "已删除你的电话号码。"
                : string.Empty;

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                Email = await UserManager.GetEmailAsync(userId),
                IsEmailConfirmed = await UserManager.IsEmailConfirmedAsync(userId),
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                IsPhoneNumberConfirmed = await UserManager.IsPhoneNumberConfirmedAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        // GET: /Manage/AddPhoneNumber
        [Route("phonenumber-create")]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAddPhoneNumber(AddPhoneNumberViewModel model)
        {
            //add by zhuyong
            var getUser = await UserManager.FindByPhoneAsync(model.Number);
            if (getUser != null)
            {
                ModelState.AddModelError(string.Empty, "该手机号已被注册");
                return View("AddPhoneNumber", model);
            }

            if (!ModelState.IsValid)
            {
                return View("AddPhoneNumber", model);
            }

            // 生成令牌并发送该令牌
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = string.Format("欢迎注册，您的手机验证码为{0}，请勿泄露。", code)
                };
                await UserManager.SmsService.SendAsync(message);
            }

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        // GET: /Account/SendPhoneConfirmationToken
        public async Task<ActionResult> SendPhoneConfirmationToken()
        {
            // 生成令牌并发送该令牌
            var userId = User.Identity.GetUserId();
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

            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = number });
        }

        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Profile");
        }

        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", "Profile");
        }

        // GET: /Manage/VerifyPhoneNumber
        [Route("verify-phonenumber")]
        public ActionResult VerifyPhoneNumber(string phoneNumber)
        {
            // 通过 SMS 提供程序发送短信以验证电话号码
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveVerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VerifyPhoneNumber", model);
            }

            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            ModelState.AddModelError(string.Empty, "验证码错误");
            return View("VerifyPhoneNumber", model);
        }

        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        // GET: /Manage/ChangePassword
        [Route("changepassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", model);
            }

            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }

                var resultViewModel = new ResultViewModel
                {
                    ControllerName = "profile",
                    ActionName = "index"
                };

                return RedirectToAction("ChangePasswordSuccess", resultViewModel);
            }

            AddErrors(result);
            return View("ChangePassword", model);
        }

        [AllowAnonymous]
        [Route("changepassword-success")]
        public ActionResult ChangePasswordSuccess(ResultViewModel model)
        {
            ViewBag.Message = "修改成功";
            return View("Success_Info", model);
        }

        // GET: /Manage/SetPassword
        [Route("password-set")]
        public ActionResult SetPassword()
        {
            return View();
        }

        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveSetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }

                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }

                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View("SetPassword", model);
        }

        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "已删除外部登录名。"
                : message == ManageMessageId.Error ? "出现错误。"
                : string.Empty;

            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }

            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // 请求重定向至外部登录提供程序，以链接当前用户的登录名
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Profile"), User.Identity.GetUserId());
        }

        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }

            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }

            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }

            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}