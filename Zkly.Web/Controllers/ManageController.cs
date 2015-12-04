using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using Zkly.BLL.Repositories;
using Zkly.BLL.ViewModels;
using Zkly.Common;
using Zkly.Common.Extension;
using Zkly.DAL.Models;

namespace Zkly.Web.Controllers
{
    public class ManageController : BaseController
    {
        #region//Repositories

        private ResourceRepository _resourceRepository = new ResourceRepository();
        private PreferenceRepository _preferenceRepository = new PreferenceRepository();

        #endregion

        #region //投资者个人投资偏好设置

        [Route("invester-preference")]
        public ActionResult InvestorPreference()
        {
            var preference = _preferenceRepository.GetPreferenceByUserId(User.Identity.GetUserId());
            this.ViewData["datas"] = this.GetSelectList(preference);

            var model = new InvestorPreferenceViewModel
                {
                    Id = preference == null ? 0 : preference.Id,
                    OrgPreferences = GetOrgPreferenceViewModels(preference),
                    IndustryPreferences = GetIndustryPreferenceViewModels(preference),
                    FileId = preference == null ? 0 : preference.FileId
                };

            return View("InvestorPreference", model);
        }

        public List<SelectListItem> GetSelectList(InvestorPreference pre)
        {
            string selectedValue = "100-1000";

            if (pre != null)
            {
                selectedValue = pre.Upper == 0 ? pre.Lower.ToString() : string.Format("{0}-{1}", pre.Lower, pre.Upper);
            }

            var list = new List<SelectListItem>
            {
                new SelectListItem { Text = "100万~1000万", Value = "100-1000" },
                new SelectListItem { Text = "1000万~3000万", Value = "1000-3000" },
                new SelectListItem { Text = "3000万~1亿", Value = "3000-10000" },
                new SelectListItem { Text = "1亿~3亿", Value = "10000-30000" },
                new SelectListItem { Text = "3亿~5亿", Value = "30000-50000" },
                new SelectListItem { Text = ">5亿", Value = "50000" }
            };

            foreach (var item in list)
            {
                if (item.Value == selectedValue)
                {
                    item.Selected = true;
                }
            }

            return list;
        }

        public List<OrgPreferenceViewModel> GetOrgPreferenceViewModels(InvestorPreference pre)
        {
            var list = new List<OrgPreferenceViewModel>();

            var allOrgs = _resourceRepository.GetOrganizations();

            var setting = pre == null || pre.OrgPreference == null
                ? new List<string>()
                : pre.OrgPreference.Split(',').ToList();

            foreach (var item in allOrgs)
            {
                list.Add(new OrgPreferenceViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsAssign = setting.Contains(item.Id.ToString())
                });
            }

            return list;
        }

        public List<IndustryViewModel> GetIndustryPreferenceViewModels(InvestorPreference pre)
        {
            var allIndus = _resourceRepository.GetIndustries();

            var list = new List<IndustryViewModel>();
            var setting = pre == null || pre.IndustryPreference == null
                ? new List<string>()
                : pre.IndustryPreference.Split(',').ToList();

            foreach (var item in allIndus)
            {
                list.Add(new IndustryViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsAssign = setting.Contains(item.Id.ToString())
                });
            }

            return list;
        }

        [HttpPost]
        public ActionResult SavePreperence(InvestorPreferenceViewModel model, string[] orgPreferences, string[] industryPreferences)
        {
            var scale = model.InvestmentScales;
            long upper = 0;
            long lower = 0;
            var index = scale.IndexOf("-");
            int imgLimitedUploadCapacity = 2; //2M

            //验证是否是图片
            if (!FileHelper.ImageContentTypeList().Contains(model.BusinessLicense.ContentType.ToLower()))
            {
                var preference = _preferenceRepository.GetPreferenceByUserId(User.Identity.GetUserId());
                this.ViewData["datas"] = this.GetSelectList(preference);
                model.OrgPreferences = GetOrgPreferenceViewModels(preference);
                model.IndustryPreferences = GetIndustryPreferenceViewModels(preference);

                ModelState.AddModelError("BusinessLicense", "图片格式错误");
                return this.View("InvestorPreference", model);
            }

            //限制大小2M
            if (model.BusinessLicense.IsOverlimitedCapacity(imgLimitedUploadCapacity))
            {
                var preference = _preferenceRepository.GetPreferenceByUserId(User.Identity.GetUserId());
                this.ViewData["datas"] = this.GetSelectList(preference);
                model.OrgPreferences = GetOrgPreferenceViewModels(preference);
                model.IndustryPreferences = GetIndustryPreferenceViewModels(preference);

                ModelState.AddModelError("BusinessLicense", "图片超过2M");
                return this.View("InvestorPreference", model);
            }

            if (index > 0)
            {
                var l = scale.Substring(0, index);
                var u = scale.Substring(index + 1);
                lower = long.Parse(l);
                upper = long.Parse(u);
            }
            else
            {
                lower = long.Parse(scale);
            }

            bool result;
            if (model.Id == 0)
            {
                var preference = new InvestorPreference
                {
                    Id = model.Id,
                    UserId = User.Identity.GetUserId(),
                    OrgPreference = orgPreferences == null ? null : string.Join(",", orgPreferences),
                    IndustryPreference = industryPreferences == null ? null : string.Join(",", industryPreferences),
                    Lower = lower,
                    Upper = upper
                };

                result = _preferenceRepository.SavePreference(preference, model.BusinessLicense);
            }
            else
            {
                var p = _preferenceRepository.SelOneInvestorPreference(model.Id);
                p.Lower = lower;
                p.Upper = upper;
                p.OrgPreference = orgPreferences == null ? null : string.Join(",", orgPreferences);
                p.IndustryPreference = industryPreferences == null ? null : string.Join(",", industryPreferences);

                result = _preferenceRepository.SavePreference(p, model.BusinessLicense);
            }

            ViewBag.Message = "保存成功";
            return View(result ? "Success_Info" : "Failure", new ResultViewModel { ActionName = "Index", ControllerName = "Home" });
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_preferenceRepository != null)
                {
                    _preferenceRepository.Dispose();
                    _preferenceRepository = null;
                }

                if (_resourceRepository != null)
                {
                    _resourceRepository.Dispose();
                    _resourceRepository = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}