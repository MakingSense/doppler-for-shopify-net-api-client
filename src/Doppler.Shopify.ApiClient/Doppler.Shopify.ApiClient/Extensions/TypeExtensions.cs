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
            var props = type.GetProperties().ToList();
            return props;
        }
    }
}