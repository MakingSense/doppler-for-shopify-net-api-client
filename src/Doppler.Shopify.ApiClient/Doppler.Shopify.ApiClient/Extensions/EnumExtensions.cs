using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Doppler.Shopify.ApiClient
{
    /// <summary>
    /// Enum Extension Method
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Reads and uses the enum's <see cref="EnumMemberAttribute"/> for serialization.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSerializedString(this Enum input)
        {
            string name = input.ToString();
            var info = input.GetType().GetMembers().Where(i => i.Name == name);

            if (info.Count() > 0)
            {
                var attribute = info.First().GetCustomAttributes(typeof(EnumMemberAttribute), true);
                    
                if (attribute != null && attribute.Length > 0)
                {
                    return ((EnumMemberAttribute)attribute[0]).Value;
                }
            }

            return name.ToLower();
        }

        /// <summary>
        /// Convert list of Enums to a comma seperated string
        /// </summary>
        public static string EnumListToString<T>(IEnumerable<T> enumList)
        {
            var list = new List<string>();

            if (enumList != null && enumList.Any())
            {
                foreach (var enumItem in enumList)
                {
                    list.Add(EnumExtensions.ToSerializedString(enumItem as Enum));
                }
            }
            return string.Join(",", list);
        }
    }
}
