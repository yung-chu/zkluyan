using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.Common.Extension
{
    public static class BaseTypeExt
    {
        /// <summary>
        /// check if the value is equals with any of the compare values.
        /// </summary>
        /// <param name="theSource">the source value</param>
        /// <param name="compares">compare values</param>
        /// <returns>true or false</returns>
        public static bool EqualsAny<T>(this T theSource, params T[] compares) where T : struct
        {
            return compares.Any(s => theSource.Equals(s));
        }
    }
}
