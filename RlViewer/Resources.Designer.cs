﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.0
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RlViewer {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RlViewer.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Локационные файлы |*.rl4;*.brl4;*.rl8;*.r;*.raw|Файлы РЛИ МРК2 (*.rl4;*.rl8)|*.rl4;*.rl8|Файлы РЛИ Банк-РЛ (*.brl4)|*.brl4|Бортовые файлы РЛИ МРК2 (*.r)|*.r|Файлы МРК411 без заголовка (*.raw)|*.raw.
        /// </summary>
        internal static string OpenFilter {
            get {
                return ResourceManager.GetString("OpenFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файлы МРК411 без заголовка (*.raw)|*.raw|Файлы изображений (*.bmp)|*.bmp.
        /// </summary>
        internal static string RawSaveFilter {
            get {
                return ResourceManager.GetString("RawSaveFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файлы РЛИ МРК2 (*.rl8)|*.rl8|Файлы МРК411 без заголовка (*.raw)|*.raw|Файлы изображений (*.bmp)|*.bmp.
        /// </summary>
        internal static string Rl8SaveFilter {
            get {
                return ResourceManager.GetString("Rl8SaveFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Файлы РЛИ МРК2 (*.rl4)|*.rl4|Файлы РЛИ Банк-РЛ (*.brl4)|*.brl4|Файлы МРК411 без заголовка (*.raw)|*.raw|Файлы изображений (*.bmp)|*.bmp.
        /// </summary>
        internal static string SaveFilter {
            get {
                return ResourceManager.GetString("SaveFilter", resourceCulture);
            }
        }
    }
}
