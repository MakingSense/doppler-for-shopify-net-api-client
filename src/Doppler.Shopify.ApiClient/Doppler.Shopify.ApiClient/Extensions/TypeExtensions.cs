using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Doppler.Shopify.ApiClient
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetAllDeclaredProperties(this Type type)
        {
            // .NET Core did not add Type.GetProperties until 1.5, so we need a recursive function
            // to return a list of this properties DeclareProperties, and its base type's DeclaredProperties.
            var props = type.GetProperties().ToList();

            if (type.BaseType != null)
            {
                props.AddRange(type.BaseType.GetAllDeclaredProperties());
            }

            return props;
        }
    }
}