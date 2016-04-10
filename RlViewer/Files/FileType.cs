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
        /// Формат файла РЛИ без служебной информации
        /// </summary>
        raw,
        /// <summary>
        /// Формат файла бортового РЛИ
        /// </summary>
        r,
        /// <summary>
        /// Формат файла РГГ МРК2
        /// </summary>
        k,
        /// <summary>
        /// Формат изображений для вывода
        /// </summary>
        bmp
    }

    public static class EnumExt
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            T res;
            if(Enum.TryParse<T>(value, true, out res))
            {
                return res;
            }
            throw new NotSupportedException("attempt to cast from unsupported type");
        }
    }
}
