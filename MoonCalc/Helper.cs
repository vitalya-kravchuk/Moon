using System;
using System.Reflection;

namespace MoonCalc
{
    /// <summary>
    /// Вспомогательные функции
    /// </summary>
    class Helper
    {
        /// <summary>
        /// Конвертация дробного числа в целое 64-битное число
        /// </summary>
        public static Int64 DoubleToInt64(double d)
        {
            return Convert.ToInt64(Math.Floor(d));
        }

        /// <summary>
        /// Конвертация дробного числа в целое 32-битное число
        /// </summary>
        public static Int32 DoubleToInt32(double d)
        {
            return Convert.ToInt32(Math.Floor(d));
        }

        /// <summary>
        /// Скопировать соответствующие поля из объекта в объект
        /// </summary>
        /// <param name="sourceObject">Объект-источник</param>
        /// <param name="destObject">Объект-назначение, который предварительно должен быть создан</param>
        public static void CopyObject<T>(object sourceObject, ref T destObject)
        {
            if (sourceObject == null || destObject == null)
                return;

            Type sourceType = sourceObject.GetType();
            Type targetType = destObject.GetType();

            foreach (PropertyInfo p in sourceType.GetProperties())
            {
                PropertyInfo targetObj = targetType.GetProperty(p.Name);
                if (targetObj == null)
                    continue;

                targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
            }
        }
    }
}
