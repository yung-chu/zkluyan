using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Repository;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zkly.Common.Log;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class DataDictionaryReponsitory: RepositoryBase<UserDbContext>
    {
        private UserDbContext context = new UserDbContext();
        #region 数据字典读取操作
        /// <summary>
        /// 得到单个数据字典对象
        /// </summary>
        /// <param name="dataDictionaryId">主键id</param>
        /// <param name="dataDictionaryType">类型</param>
        /// <returns>返回当前数据字典对象</returns>
        public DataDictionary GetDataDictionary(long dataDictionaryId, string dataDictionaryType)
        {
           return FirstOrDefault<DataDictionary>(
                p => p.DataDictionaryId == dataDictionaryId && p.DataDictionaryType == dataDictionaryType);
        }

        /// <summary>
        /// 得到数据字典集合
        /// </summary>
        /// <param name="dataDictionaryType">枚举数据字典</param>
        /// <returns>返回数据字典集合</returns>
        public List<DataDictionary> GetDataDictionaryList(string dataDictionaryType)
        {
            return GetAll<DataDictionary>().Where(p => p.DataDictionaryType == dataDictionaryType).ToList();    
        }

        /// <summary>
        /// 得到数据字典集合
        /// </summary>
        /// <param name="dataDictionaryType">枚举数据字典</param>
        /// <param name="parentId">父类型</param>
        /// <returns>返回数据字典集合</returns>
        public List<DataDictionary> GetDataDictionaryList(string dataDictionaryType, int parentId)
        {
            return
                GetAll<DataDictionary>().Where(p => p.DataDictionaryType == dataDictionaryType && p.ParentId == parentId)
                    .ToList();
        }

        public bool UpdDataDictionary(DataDictionary dataDictionaries)
        {
            context.Entry(dataDictionaries).State = EntityState.Modified;
            context.SaveChanges();
            return true;
        }

        #endregion
    }
}
