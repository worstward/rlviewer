﻿using System;
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
        /// Формат файла РЛИ (4 байта на отсчет)
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
        bmp,
        /// <summary>
        /// Формат файла рли (8 байт на отсчет)
        /// </summary>
        rl8,
        /// <summary>
        /// Формат файла РГГ Банк-РЛ
        /// </summary>
        ba
    }

}
