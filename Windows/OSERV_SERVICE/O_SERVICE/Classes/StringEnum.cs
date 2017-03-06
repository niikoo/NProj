using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace OSERV_BASE.Classes
{
    /// <summary>
    /// Code copied from http://stackoverflow.com/questions/424366/c-sharp-string-enums (by Steve Mitcham and fixed by Paula Bean)
    /// 
    /// How to create an string enum:
    /// 
    /// [TypeConverter(typeof(CustomEnumTypeConverter<MyEnum>))]
    /// public enum MyEnum
    /// {
    ///     // The custom type converter will use the description attribute
    ///     [Description("A custom description")]
    ///     ValueWithCustomDescription,
    /// 
    ///     // This will be exposed exactly.
    ///     Exact
    /// }
    /// </summary>
    public static class StringEnum {
        public static string GetDescription<T>(this object enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
    }
}
