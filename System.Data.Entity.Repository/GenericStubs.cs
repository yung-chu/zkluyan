using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Entity.Repository
{
    public class Stubs<T>
    {
        /// <summary>
        /// Create a list of type T stubs with filled in Primary Keys. If no primarykeycolumn name is specified than the column "Id", "[Classname]Id", "[Classname]ID" or the column with a [Key] attribute is searched for.
        /// </summary>
        /// <param name="primaryKeys"></param>
        /// <param name="primaryKeyColumn"></param>
        /// <returns></returns>
        public static List<T> Create(int[] primaryKeys, string primaryKeyColumn = null)
        {
            List<T> result = new List<T>();
            var instanceOfT = typeof(T);
            var instanceName = instanceOfT.Name;
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in properties)
            {
                // For now only use integer PK columns
                if (p.PropertyType != typeof(int)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                if(p.Name.ToLower() != string.Concat(instanceName.ToLower(), "id")){continue;}

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }


                foreach (var pk in primaryKeys)
                {
                    var instance = Activator.CreateInstance<T>();

                    var currentPropertyValue = (int)p.GetValue(instance, null);

                    if (currentPropertyValue == null || currentPropertyValue == 0)
                    {
                        p.SetValue(instance, pk, null);
                    }

                    result.Add(instance);
                }
                
            }

            return result;
        }
    }
}
