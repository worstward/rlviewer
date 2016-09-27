using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RlViewer
{
    static class ConvertHelper
    {

        /// <summary>
        /// Преобразует массив байт со структурой SYSTEMTIME в объект DateTime
        /// </summary>C
        /// <param name="timeArray">16 байт структуры SYSTEMTIME</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this byte[] timeArray)
        {
            if (timeArray.Length != 16) throw new ArgumentException();

            

            
            short year = BitConverter.ToInt16(timeArray, 0);
            short month = BitConverter.ToInt16(timeArray, 2);
            short dayOfWeek = BitConverter.ToInt16(timeArray, 4);
            short day = BitConverter.ToInt16(timeArray, 6);
            short hour = BitConverter.ToInt16(timeArray, 8);
            short minute = BitConverter.ToInt16(timeArray, 10);
            short second = BitConverter.ToInt16(timeArray, 12);
            short millisecond = BitConverter.ToInt16(timeArray, 14);


            DateTime dateTime;
            try
            {
                dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
            }
            catch
            {
                dateTime = default(DateTime);
            }
            return dateTime;
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


        public static string ToPolarizationType(this byte type)
        {
            //0 – ВВ, 1 – ГГ, 2 – ВГ, 3 – ГВ

            string sType;

            switch (type)
            {
                case 0:
                    sType = "ВВ";
                    break;
                case 1:
                    sType = "ГГ";
                    break;
                case 2:
                    sType = "ВГ";
                    break;
                case 3:
                    sType = "ГВ";
                    break;
                default:
                    sType = "Не определено";
                    break;
            }
            return sType;
        }


        public static string ToSynchronizerBoard(this byte board)
        {
            string sType;

            switch (board)
            {
                case 1:
                    sType = "Правый";
                    break;
                case 2:
                    sType = "Левый";
                    break;
                case 3:
                    sType = "Попеременно с правого";
                    break;
                case 4:
                    sType = "Попеременно с левого";
                    break;
                default:
                    sType = "Не определено";
                    break;
            }
            return sType;
        }

        public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        public static string ToOverviewMode(this RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes mode)
        {
            string sType;
            switch (mode)
            {
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.SDC1:
                    sType = "СДЦ1/Обзор";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.SDC2:
                    sType = "СДЦ2";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.Relef:
                    sType = "Рельеф";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.NoniusGG:
                    sType = "НониусГГ";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.NoniusGV:
                    sType = "НониусГВ";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.NoniusVG:
                    sType = "НониусВГ";
                    break;
                case RlViewer.Headers.Concrete.R.SynchronizerHeader.OverviewModes.NoniusVV:
                    sType = "НониусВВ";
                    break;
                default:
                    sType = "Не определено";
                    break;
            }
            return sType;
        }


        public static string ToRliFileType(this RlViewer.Headers.Concrete.Rl4.SampleType type)
        {
            string sType;

            switch (type)
            {
                case RlViewer.Headers.Concrete.Rl4.SampleType.Float:
                    sType = "В число с плавающей точкой";
                    break;
                case RlViewer.Headers.Concrete.Rl4.SampleType.Complex:
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
            float readable;

            const float kb = 1024f;
            const float mb = 1048576f;
            const float gb = 1073741824f;


            if (value >= 1073741824)
            {
                sizeSuffix = "Gb";
                readable = value / gb;
            }
            else if (value >= 1048576)
            {
                sizeSuffix = "Mb";
                readable = value / mb;
            }
            else if (value >= 1024)
            {
                sizeSuffix = "Kb";
                readable = value / kb;
            }
            else
            {
                sizeSuffix = "b";
                readable = 0;
            }

            return string.Format("{0}{1}", readable.ToString("0.## "), sizeSuffix);
        }

    }
}
