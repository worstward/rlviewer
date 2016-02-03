using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    public enum FileType
    {
        /// <summary>
        /// Формат файла РЛИ Банк-РЛ
        /// </summary>
        brl4,
        /// <summary>
        /// Формат файла РЛИ
        /// </summary>
        rl4,
        /// <summary>
        /// Формат файла РГГ МРК2
        /// </summary>
        k
    }

    public static class EnumExt
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
