using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class InvestTempFolderRepository : RepositoryBase<UserDbContext>
    {
        private UserDbContext db = new UserDbContext();
        private static object _ojb = new object();

        public List<InvestTempFolder> ListInvestTempFolder(EInvestAuditStage stage)
        {
            return GetAll<InvestTempFolder>(p=>p.Invest, p=>p.User).Where(p => p.Invest.Stage == stage).ToList();
        }

        public bool AddOrUpdateMultiple(List<InvestTempFolder> list)
        {
            lock (_ojb)
            {
                foreach (var item in list)
                {
                    if (FirstOrDefault<InvestTempFolder>(p => p.InvestId == item.InvestId, p=>p.User, p=>p.Invest) != null)
                    {
                        continue;
                    }

                    AddInvestTempFolder(item);
                }
            }

            return true;
        }

        public bool AddInvestTempFolder(InvestTempFolder model)
        {
            db.InvestTempFolders.Add(model);
            db.SaveChanges();
            return true;
        }

        public InvestTempFolder GetInvestTempFolder(int id)
        {
            return FirstOrDefault<InvestTempFolder>(p => p.Id == id, p=>p.User, p=>p.Invest);
        }

        public bool DeleteInvestTempFolder(int id)
        {
            var model = GetInvestTempFolder(id);
            if (model != null)
            {
                db.InvestTempFolders.Remove(model);
                db.SaveChanges();
            }

            return true;
        }

        public bool DeleteInvestTempFolder(List<string> list)
        {
            var model = GetAll<InvestTempFolder>(p => p.User, p=>p.Invest).Where(p => list.Contains(p.InvestId.ToString())).ToList();
            db.InvestTempFolders.RemoveRange(model);
            db.SaveChanges();
        
            return true;
        }
    }
}
