using System.ComponentModel;
using System.Reflection;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Helper для работы с <see cref="DescriptionAttribute"/>
    /// </summary>
    public static class DescipriptionAttributeHelper
    {
        /// <summary>
        /// Возвращает значение атрибута.
        /// </summary>
        /// <typeparam name="T">Тип перечисления.</typeparam>
        /// <param name="source">Источник.</param>
        /// <returns></returns>
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return source.ToString();
            }
        }
    }
}
