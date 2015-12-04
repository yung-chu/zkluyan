using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Zkly.BLL.Repositories;

namespace Zkly.BLL.ViewModels
{
    public class IndustryDropdown
    {
        private ResourceRepository _resourceRepository = new ResourceRepository();

        public List<SelectListItem> Items { get; set; }

        public IndustryDropdown(string selectedValue = null)
        {
            var industries = _resourceRepository.GetIndustries();

            Items = new List<SelectListItem> { new SelectListItem { Text = "请选择", Value = "请选择" } };

            foreach (var industry in industries)
            {
                Items.Add(new SelectListItem { Text = industry.Name, Value = industry.Name });
            }

            foreach (var item in Items)
            {
                if (selectedValue != null && selectedValue == item.Value)
                {
                    item.Selected = true;
                }
            }
        }
    }
}
