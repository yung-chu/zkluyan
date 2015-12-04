using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zkly.BLL.Repositories;

namespace Zkly.Admin.Web.Models
{
    public class InvestDropdownModel
    {
        private InvestRepository _investRepository = new InvestRepository();

        public List<SelectListItem> Items { get; set; }

        public InvestDropdownModel(string selectedValue = null)
        {
            var invests = _investRepository.GetOnRoadshowInvest();

            Items = new List<SelectListItem> { new SelectListItem { Text = "请选择", Value = "0" } };

            if (invests != null)
            {
                var selectItem = new SelectListItem();
                foreach (var item in invests)
                {
                    selectItem = new SelectListItem();
                    selectItem.Value = item.Id.ToString();
                    selectItem.Text = item.FirstAuditInfo.ProjectName;
                    if (null != selectedValue && selectedValue == item.Id.ToString())
                    {
                        selectItem.Selected = true;
                    }

                    Items.Add(selectItem);

                    //Items.Add(new SelectListItem
                    //{
                    //    Value = item.Id.ToString(),
                    //    Text = item.FirstAuditInfo.ProjectName
                    //});
                }
                
                //foreach (var item in Items)
                //{
                //    if (selectedValue != null && selectedValue == item.Value)
                //    {
                //        item.Selected = true;
                //    }
                //}
            }            
        }
    }
}