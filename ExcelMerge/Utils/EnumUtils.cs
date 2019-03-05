using System;
using System.ComponentModel;
using System.Linq;

namespace ExcelMerge.Utils
{
    public static class EnumUtils
    {
        public static string[] GetDescription<T>() where T : struct
        {
            var descriptions = Enum.GetValues(typeof(T)).Cast<T>();

            return descriptions.Select(s => GetDescriptionFromValue(s)).ToArray();
        }

        public static string GetDescriptionFromValue<T>(T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                return null;
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }
    }
}