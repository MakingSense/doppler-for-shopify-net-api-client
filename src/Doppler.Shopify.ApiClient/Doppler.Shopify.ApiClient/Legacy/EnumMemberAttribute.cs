using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Runtime.Serialization
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class EnumMemberAttribute : Attribute
    {
        public string Value { get; set; }
    }
}
