using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

using Zkly.BLL.Account;
using Zkly.BLL.ViewModels;
using Zkly.Common.Mvc.Attribute;

namespace Zkly.Admin.Web.Controllers
{
    [Route("Profile/{action}")]
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
        [RestoreModelState]
        [Route("profile")]
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "已更改你的密码。"
                : message == ManageMessageId.SetPasswordSuccess ? "已设置你的密码。"
                : message == ManageMessageId.SetTwoFactorSuccess ? "已设置你的双重身份验证提供程序。"
                : message == ManageMessageId.Error ? "出现错误。"
                : message == ManageMessageId.AddPhoneSuccess ? "已添加你的电话号码。"
                : message == ManageMessageId.RemovePhoneSuccess ? "已删除你的电话号码。"
                : "";

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

        // GET: /Profile/AddPhoneNumber
        [Route("phonenumber-add")]
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        // POST: /Profile/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveAddPhoneNumber(AddPhoneNumberViewModel model)
        {
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

        // POST: /Profile/EnableTwoFactorAuthentication
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

        // POST: /Profile/DisableTwoFactorAuthentication
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

        // GET: /Profile/VerifyPhoneNumber
        [RestoreModelState]
        [Route("verify-phonenumber")]
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);

            // 通过 SMS 提供程序发送短信以验证电话号码
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        // POST: /Profile/VerifyPhoneNumber
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
            ModelState.AddModelError("", "无法验证电话号码");
            return View("VerifyPhoneNumber", model);
        }

        // GET: /Profile/RemovePhoneNumber
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

        // GET: /Profile/ChangePassword
        [Route("changepassword")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Profile/ChangePassword
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

                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            AddErrors(result);
            return View("ChangePassword", model);
        }
        
        // GET: /Profile/SetPassword
        [Route("set-password")]
        public ActionResult SetPassword()
        {
            return View();
        }

        // POST: /Profile/SetPassword
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
            return View(model);
        }

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
                ModelState.AddModelError("", error);
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