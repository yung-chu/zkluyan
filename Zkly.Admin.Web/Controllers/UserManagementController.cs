using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Glimpse.AspNet.Tab;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using Zkly.BLL.ViewModels;
using Zkly.Common.Config;
using Zkly.Common.Extension;
using Zkly.Common.Mvc.Attribute;
using Zkly.Common.Utils;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    public class UserManagementController : BaseController
    {
        private const int PageSize = 10;

        // GET: UserManagement
        [RestoreModelState]
        [Route("user-{role=Enterprise}s/{page?}")]
        public ActionResult Index(EUserRole role, int? page)
        {
            ViewBag.Role = role;

            var roleId = RoleManager.FindByName(role.ToString()).Id;
            var query = UserManager.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId)).OrderBy(u => u.UserName);
            var model = query.ToPagedList(page ?? 1, PageSize);
            return View(model);
        }

        [HttpPost]
        [Route("users-search/{page?}")]
        public async Task<ActionResult> Search(int? page, string userId, string userName, string email, string phoneNumber)
        {
            //todo: 添加组合查询，角色查询，分页
            var users = new List<ApplicationUser>();
            ApplicationUser user = null;
            if (!string.IsNullOrEmpty(userId))
            {
                user = await UserManager.FindByIdAsync(userId);
            }
            else if (!string.IsNullOrEmpty(userName))
            {
                user = await UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    users.AddRange(UserManager.Users.Where(u => u.UserName.Contains(userName)));
                }
            }
            else if (!string.IsNullOrEmpty(email))
            {
                user = await UserManager.FindByEmailAsync(email);
                if (user == null)
                {
                    users.AddRange(UserManager.Users.Where(u => u.Email.Contains(email)));
                }
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                user = await UserManager.FindByPhoneAsync(phoneNumber);
                if (user == null)
                {
                    users.AddRange(UserManager.Users.Where(u => u.PhoneNumber.Contains(phoneNumber)));
                }
            }

            if (user == null)
            {
                if (users.Count == 0)
                {
                    ModelState.AddModelError("", "无法找到该用户，请重新查询。");
                }

                return View("Index", users.ToPagedList(page ?? 1, PageSize));
            }

            return View("Details", user);
        }

        // GET: UserManagement/Details/5
        [Route("user-detail/{id}")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var view = Request.IsAjaxRequest() ? "_Details" : "Details";
            return View(view, applicationUser);
        }

        // GET: UserManagement/Create
        [Route("{role=enterprise}-user-create")]
        public ActionResult Create(EUserRole? role)
        {
            var model = new RegisterViewModel { Role = role.GetValueOrDefault() };
            return View(model);
        }

        // POST: UserManagement/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCreate(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var getUser = await UserManager.FindByPhoneAsync(model.PhoneNumber);
                if (getUser != null)
                {
                    ModelState.AddModelError("", "该手机号已经存在，请从新输入！");
                    return View("Create", model);
                }

                var user = new ApplicationUser { UserName = model.UserName, DisplayName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, model.Role);
                    if (result.Succeeded)
                    {
                        AddSuccessMessage("成功：", string.Format("用户（{0}） 已成功创建。", model.UserName));
                        return RedirectToAction("Details", new { user.Id });
                    }
                }

                AddErrors(result);
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return RedirectToAction("Create", model);
        }

        [Route("{role=enterprise}-users-create")]
        public ActionResult CreateMultiple(EUserRole role)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveCreateMultiple(HttpPostedFileBase excelFile, EUserRole role)
        {
            var users = new Dictionary<int, ApplicationUser>();
            if (ModelState.IsValid)
            {
                if (excelFile == null)
                {
                    ModelState.AddModelError("", "请选择上传的Excel文件！");
                    return RedirectToAction("CreateMultiple", users);
                }

                //读取上传excel 的数据
                var list = ExcelUtil.ReadList<RegisterViewModel>(excelFile.InputStream, excelFile.FileName.ToLower().EndsWith(".xlsx"));
                var recordId = 0;
                foreach (var item in list)
                {
                    recordId++;
                    try
                    {
                        var user = new ApplicationUser
                        {
                            UserName = item.UserName,
                            DisplayName = item.UserName,
                            Email = item.Email,
                            PhoneNumber = item.PhoneNumber
                        };
                        var result = await UserManager.CreateAsync(user, item.Password);
                        if (result.Succeeded)
                        {
                            users.Add(recordId, user);
                            result = await UserManager.AddToRoleAsync(user.Id, role);
                        }

                        if (!result.Succeeded)
                        {
                            var key = string.Format("记录{0}创建失败：", recordId);
                            ModelState.AddModelError(key, string.Format("用户名：{0}，电子邮箱：{1}，手机号码：{2}，密码：{3}", item.UserName, item.Email, item.PhoneNumber, item.Password));
                            AddErrors(key, result);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Format("记录{0}创建异常：", recordId), ex.Message);
                    }
                }

                AddSuccessMessage("导入完成：", string.Format("已读取{0}条记录，成功创建{1}个新用户。", list.Count, users.Count));
            }

            return RedirectToAction("CreateMultiple", users);
        }

        // GET: UserManagement/Edit/5
        [RestoreModelState]
        [Route("user-edit/{id}")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var applicationUser = await UserManager.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            return View(applicationUser);
        }

        // POST: UserManagement/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEdit(ApplicationUser user, EUserRole role)
        {
            var changes = new List<string>();
            var applicationUser = user;
            if (ModelState.IsValid)
            {
                applicationUser = await UserManager.FindByIdAsync(user.Id);
                if (applicationUser.UserName != user.UserName)
                {
                    applicationUser.UserName = user.UserName;
                    changes.Add("用户名");
                }

                if (applicationUser.DisplayName != user.DisplayName)
                {
                    applicationUser.DisplayName = user.DisplayName;
                    changes.Add("用户昵称");
                }

                if (applicationUser.PhoneNumber != user.PhoneNumber)
                {
                    applicationUser.PhoneNumberConfirmed = false;
                    applicationUser.PhoneNumber = user.PhoneNumber;
                    changes.Add("手机号码");
                }

                if (applicationUser.Email != user.Email)
                {
                    applicationUser.EmailConfirmed = false;
                    applicationUser.Email = user.Email;
                    changes.Add("电子邮箱");
                }

                if (applicationUser.LockoutEnabled != user.LockoutEnabled)
                {
                    applicationUser.LockoutEnabled = user.LockoutEnabled;
                    changes.Add("允许锁定");
                }

                IdentityResult result = null;
                if (changes.Count > 0)
                {
                    result = await UserManager.UpdateAsync(applicationUser);
                }

                if (result == null || result.Succeeded)
                {
                    if (applicationUser.Roles.Count == 0)
                    {
                        result = await UserManager.AddToRoleAsync(applicationUser.Id, role);
                        changes.Add("添加角色");
                    }
                    else if (!await UserManager.IsInRoleAsync(applicationUser.Id, role))
                    {
                        result = await UserManager.RemoveAllRolesAsync(applicationUser.Id);
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddToRoleAsync(applicationUser.Id, role);
                            changes.Add("变更角色");
                        }
                    }

                    if (result == null)
                    {
                        AddSuccessMessage("已取消：", string.Format("用户（{0}） 资料未做任何修改。", applicationUser.UserName));
                        return RedirectToAction("Details", new { applicationUser.Id });
                    }

                    if (result.Succeeded)
                    {
                        AddSuccessMessage(string.Format("用户（{0}） 资料已修改：", applicationUser.UserName), changes.Join("，"));
                        return RedirectToAction("Details", new { applicationUser.Id });
                    }
                }

                AddErrors(result);
            }

            return RedirectToAction("Edit", applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RestoreModelState]
        [Route("user-delete/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var applicationUser = await UserManager.FindByIdAsync(id);
            if (!AppSettings.AllowUserDeletion)
            {
                ModelState.AddModelError("", "当前模式下禁止删除用户帐号！");
                return View("Details", applicationUser);
            }

            var result = await UserManager.DeleteAsync(applicationUser);

            if (result.Succeeded)
            {
                AddSuccessMessage("成功：", string.Format("用户（{0}） 已成功删除。", applicationUser.UserName));
                return RedirectToAction("Index");
            }

            AddErrors(result);
            return View("Details", applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RestoreModelState]
        [Route("user-reset-password/{id}")]
        public async Task<ActionResult> ResetPassword(string id)
        {
            var applicationUser = await UserManager.FindByIdAsync(id);
            if (string.IsNullOrEmpty(applicationUser.PhoneNumber) || applicationUser.PhoneNumber.Length < 6 || applicationUser.PhoneNumber.Length > 11)
            {
                ModelState.AddModelError("", "该用户的手机号码无效，请先修改用户手机号码！");
                return View("Details", applicationUser);
            }

            var newPassword = "m" + applicationUser.PhoneNumber;
            IdentityResult result;
            if (await UserManager.HasPasswordAsync(applicationUser.Id))
            {
                result = await UserManager.RemovePasswordAsync(applicationUser.Id);
                if (result.Succeeded)
                {
                    result = await UserManager.AddPasswordAsync(applicationUser.Id, newPassword);
                    await UserManager.SendAllMessagesAsync(id, "您的密码已重置", "您的密码已经重置，请使用新密码登录。新密码为：" + newPassword);
                }
            }
            else
            {
                result = await UserManager.AddPasswordAsync(applicationUser.Id, newPassword);
            }

            if (result.Succeeded)
            {
                AddSuccessMessage("成功：", string.Format("用户（{0}） 密码已经重置。", applicationUser.UserName));
            }
            else
            {
                AddErrors(result);
            }

            return View("Details", applicationUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("user-lock")]
        public async Task<ActionResult> Lock(string id, bool isLock, DateTime? lockoutEndDateUtc)
        {
            var applicationUser = await UserManager.FindByIdAsync(id);
            if (!await UserManager.GetLockoutEnabledAsync(id))
            {
                await UserManager.SetLockoutEnabledAsync(id, true);
            }

            if (isLock && !lockoutEndDateUtc.HasValue)
            {
                ModelState.AddModelError("", "无效的锁定时间，请重新输入。");
                return View("Details", applicationUser);
            }

            IdentityResult result;
            if (isLock)
            {
                result = await UserManager.SetLockoutEndDateAsync(id, new DateTimeOffset(lockoutEndDateUtc.Value.ToLocalTime()));
            }
            else
            {
                result = await UserManager.SetLockoutEndDateAsync(id, DateTimeOffset.MinValue);
                if (result.Succeeded && applicationUser.AccessFailedCount > 0)
                {
                    result = await UserManager.ResetAccessFailedCountAsync(id);
                }
            }

            if (result.Succeeded)
            {
                if (isLock)
                {
                    AddSuccessMessage("成功：", string.Format("用户({0}) 已成功锁定，{1:N0}分钟后将自动解除锁定状态。", applicationUser.UserName, (lockoutEndDateUtc.Value - DateTime.Now).TotalMinutes));
                }
                else
                {
                    AddSuccessMessage("成功：", string.Format("用户({0}) 已成功解锁。", applicationUser.UserName));
                }
            }
            else
            {
                AddErrors(result);
            }

            return View("Details", applicationUser);
        }

        private void AddErrors(string key, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(key, error);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            AddErrors("", result);
        }
    }
}
