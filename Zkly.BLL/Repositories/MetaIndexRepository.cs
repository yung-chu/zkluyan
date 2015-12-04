using System;
using System.Collections.Generic;
using System.Data.Entity.Repository;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Log;
using Zkly.DAL.Context;
using Zkly.DAL.Models;

namespace Zkly.BLL.Repositories
{
    public class MetaIndexRepository : RepositoryBase<UserDbContext>
    {
        public void AddIndex(string indexFile, string indexContext)
        {
            var indexes = ParseContent(indexFile, indexContext);

            var list = Find<MetaIndex>(m => m.IndexFile == indexFile).ToList();

            var filtered = indexes.Where(item => !list.Any(l => l.VideoName == item.VideoName)).ToList();

            AddMultiple(filtered);
        }

        public MetaIndex GetMetaIndex(string indexFile, string videoName)
        {
            return FirstOrDefault<MetaIndex>(m => (m.IndexFile == indexFile && m.VideoName == videoName));
        }

        private List<MetaIndex> ParseContent(string indexFile, string indexContent)
        {
            var list = new List<MetaIndex>();

            var lines = indexContent.Split('\n');

            foreach (var line in lines)
            {
                var meta = ConvertToMetaIndex(indexFile, line);
                if (meta != null)
                {
                    list.Add(meta);
                }                
            }

            return list;
        }

        private static MetaIndex ConvertToMetaIndex(string indexFile, string line)
        {
            if (string.IsNullOrEmpty(line))
            {
                return null;
            }

            string[] vals = line.Split(',');

            return new MetaIndex
            {
                IndexFile = indexFile,
                VideoName = vals[0],
                VhallShowAddress = vals[1],
                Error = vals[2].Replace("\r", " ")
            };
        }
    }
}
