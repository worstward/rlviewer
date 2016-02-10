using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    static class Converters
    {

        /// <summary>
        /// Преобразует массив байт со структурой SYSTEMTIME в объект DateTime
        /// </summary>
        /// <param name="timeArray">16 байт структуры SYSTEMTIME</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this byte[] timeArray)
        {
            if (timeArray.Length != 16) throw new ArgumentException();
            
            short year =        BitConverter.ToInt16(timeArray, 0);
            short month =       BitConverter.ToInt16(timeArray, 2);
            short dayOfWeek =   BitConverter.ToInt16(timeArray, 4);
            short day =         BitConverter.ToInt16(timeArray, 6);
            short hour =        BitConverter.ToInt16(timeArray, 8);
            short minute =      BitConverter.ToInt16(timeArray, 10);
            short second =      BitConverter.ToInt16(timeArray, 12);
            short millisecond = BitConverter.ToInt16(timeArray, 14);

            return new DateTime(year, month, day, hour, minute, second, millisecond);
        }


        /// <summary>
        /// Преобразует объект DateTime в массив байт, содержащий структуру SYSTEMTIME
        /// </summary>
        /// <param name="dateTime">объект DateTime</param>
        /// <returns></returns>
        public static byte[] ToSystime(this DateTime dateTime)
        {
            byte[] timeArray = new byte[16];

            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Year),        0, timeArray, 0,  sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Month),       0, timeArray, 2,  sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.DayOfWeek),   0, timeArray, 4,  sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Day),         0, timeArray, 6,  sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Hour),        0, timeArray, 8,  sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Minute),      0, timeArray, 10, sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Second),      0, timeArray, 12, sizeof(short));
            Buffer.BlockCopy(BitConverter.GetBytes((short)dateTime.Millisecond), 0, timeArray, 14, sizeof(short));

            return timeArray;
        }


        public static string ToRliFileType(this byte type)
        {
            string sType;

            switch (type)
            {
                case 2:
                    sType = "В число с плавающей точкой";
                    break;
                case 3:
                    sType = "В комплексное число";
                    break;
                default:
                    sType = "Не определено";
                    break;
            }
            return sType;
        }


        public static string ToReadableFileSize(this long value)
        {
            if (value < 0) throw new ArgumentException("File size can not be less than 0");
            // Determine the suffix and readable value
            string sizeSuffix;
            double readable;

            if (value >= 0x10000000000)
            {
                sizeSuffix = "Tb";
                readable = (value >> 30);
            }
            else if (value >= 0x40000000)
            {
                sizeSuffix = "Gb";
                readable = (value >> 20);
            }
            else if (value >= 0x100000)
            {
                sizeSuffix = "Mb";
                readable = (value >> 10);
            }
            else if (value >= 0x400)
            {
                sizeSuffix = "Kb";
                readable = value;
            }
            else
            {
                return value.ToString("0 b");
            }

            readable = (readable / 1024);
            return readable.ToString("0.## ") + sizeSuffix;
        }



    }
}
