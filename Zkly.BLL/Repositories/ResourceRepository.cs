using System;
using System.Collections.Generic;
using System.Data.Entity.Repository;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class ResourceRepository : RepositoryBase<UserDbContext>
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public List<Organization> GetOrganizations()
        {
            using (ConfigDbContext dbContext = new ConfigDbContext())
            {
              return dbContext.Organizations.ToList();
            }
        }

        public List<Industry> GetIndustries()
        {
            return GetAll<Industry>().ToList();
        }
    }
}
