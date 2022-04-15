using System;
using System.Reflection;

namespace Utility
{
    public static class Common
    {
        public static T GetAttributeFromType<T>(PropertyInfo property) where T : Attribute
        {
            return (T)property.GetCustomAttribute(typeof(T), true) ?? throw new Exception("No such attribute");
        }


        /// <summary>
        /// list of codes
        /// https://docs.microsoft.com/pl-pl/dotnet/api/system.text.encodinginfo?view=netframework-4.8
        /// </summary>
        public static int TextEncodingCode { get; set; } = 1252;
    }
}