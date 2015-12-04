using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zkly.DAL.Context;
using Zkly.DAL.Models;

using MessageModel = Zkly.DAL.Models.Message;

namespace Zkly.BLL.Repositories
{
    public class MessageRepository : RepositoryBase<UserDbContext>
    {
        private UserDbContext context = new UserDbContext();

        public bool AddMessage(MessageModel message)
        {
            context.Messages.Add(message);
            context.SaveChanges();

            return true;
        }

        public async Task<bool> AddMessageAysnc(MessageModel message)
        {
            context.Messages.Add(message);
            var count = await context.SaveChangesAsync();

            return count == 1;
        }

        public List<MessageModel> GetMessages()
        {
            return GetAll<MessageModel>(p => p.User).ToList();
        }

        public MessageModel GetMessages(int id)
        {
            return FirstOrDefault<MessageModel>(p => p.Id == id, p => p.User);
        }

        public int GetMessagesCount(string userId, bool state = false)
        {
            return GetAll<MessageModel>(p => p.User).Count(p => p.UserId == userId && p.State == state);
        }

        public bool UpdateMessage(MessageModel message)
        {
            context.Entry(message).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        public bool DeleteMessage(int id)
        {
            context.Messages.Remove(FirstOrDefault<MessageModel>(p => p.Id == id, p => p.User));
            context.SaveChanges();

            return true;
        }

        public bool DeleteMessage(List<int> listId)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < listId.Count; i++)
            {
                sb.AppendFormat("'{0}',", listId[i]);
            }

            string strId = sb.ToString();
            string getValue = strId.Substring(0, strId.LastIndexOf(','));
            string sql = string.Format("delete from Messages where Id in({0})", getValue);
            int result = 0;

            using (UserDbContext db = new UserDbContext())
            {
                result = db.Database.ExecuteSqlCommand(sql);
            }

            return result > 0;
        }

        public MessageModel GetLatestMessage(string userId, bool state = false)
        {
            return GetAll<MessageModel>(p => p.User).Where(p => p.UserId == userId && p.State == state).OrderByDescending(p => p.UpdateTime).Take(1).FirstOrDefault();
        }
    }
}
