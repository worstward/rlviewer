using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer.Files
{
    public static class EnumExt
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            T res;
            if (Enum.TryParse<T>(value, true, out res))
            {
                return res;
            }

            throw new NotSupportedException("Unsupported type");
        }
    }
}
