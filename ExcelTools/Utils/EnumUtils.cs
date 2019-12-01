using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ExcelTools.App.Utils
{
    public static class EnumUtils
    {
        public static void LoadComboFromEnum<T>(this ComboBox cbx, int selectedIndex) where T : struct
        {
            var descriptions = GetDescription<T>();

            descriptions.ToList().ForEach(f => cbx.Items.Add(f));

            cbx.SelectedIndex = selectedIndex;
        }

        public static IEnumerable<string> GetDescription<T>() where T : struct
        {
            var descriptions = Enum.GetValues(typeof(T)).Cast<T>();

            return descriptions.Select(s => GetDescriptionFromValue(s));
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