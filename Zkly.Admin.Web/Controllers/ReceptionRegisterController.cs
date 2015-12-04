using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Zkly.BLL.Repositories;
using Zkly.DAL.Models;

namespace Zkly.Admin.Web.Controllers
{
    public class ReceptionRegisterController : Controller
    {
        private DataDictionaryReponsitory db = new DataDictionaryReponsitory();

        // GET: ReceptionRegister
         [Route("reception-register")]
        public ActionResult Index()
        {
           DataDictionary model= db.GetDataDictionary(1, Zkly.Common.Dictionary.Enums.DataDictionaryType.IsRegister.ToString());

            return View(model);
        }

         [Route("reception-register-display")]
         public ActionResult DisableRegister(string dataDictionaryType)
         {
             DataDictionary dataDictionaries = db.GetDataDictionary(1, dataDictionaryType);
             dataDictionaries.DataDictionaryName = "0";
             db.UpdDataDictionary(dataDictionaries);
             return RedirectToAction("Index", "ReceptionRegister");
         }

         [Route("reception-register-notdisplay")]
         public ActionResult NotDisableRegister(string dataDictionaryType)
         {
             DataDictionary dataDictionaries = db.GetDataDictionary(1, dataDictionaryType);
             dataDictionaries.DataDictionaryName = "1";
             db.UpdDataDictionary(dataDictionaries);
             return RedirectToAction("Index", "ReceptionRegister");
         }
    }
}