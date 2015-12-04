using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.Common.Log;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class LoanRepository : RepositoryBase<UserDbContext>
    {
        public IQueryable<Loan> GetLoans()
        {
            return GetAll<Loan>().OrderByDescending(m => m.ApplyTime);
        }

        public bool CreateOrUpdateLoan(Loan loan)
        {
            try
            {
                if (loan.Id == 0)
                {
                    Add(loan);
                }
                else
                {
                    Update(loan);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return false;
            }

            return true;
        }

        public List<Loan> GetLoansByUserId(string userId)
        {
            return Find<Loan>(x => x.UserId == userId).ToList();
        }

        public Loan GetLoanById(long? id)
        {
            return FirstOrDefault<Loan>(l => l.Id == id, l => l.User);
        }
    }
}
