using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Normaleezie.Helpers
{
    internal class ReflectionHelper
    {
        internal virtual List<T> ConvertList<T>(List<object> list)
        {
            if (null == list)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list.Cast<T>().ToList();
        }

        internal virtual bool IsSimpleType(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Type.GetTypeCode(type) != TypeCode.Object;
        }

        internal virtual bool IsIEnumerableType(Type type)
        {
            if (null == type)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return null != type.GetInterface("IEnumerable") && type != typeof (string);
        }
    }
}
