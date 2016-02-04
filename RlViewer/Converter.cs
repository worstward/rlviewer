using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    static class Converter
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
    }
}
