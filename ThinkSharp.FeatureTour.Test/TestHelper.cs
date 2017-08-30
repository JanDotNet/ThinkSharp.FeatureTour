using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ThinkSharp.FeatureTouring.Test.Navigation;

namespace ThinkSharp.FeatureTouring.Test
{
    public static class TestHelper
    {
        public static TFieldType GetPrivateField<TFieldType>(this object instance, string fieldName) where TFieldType : class
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            FieldInfo field = instance.GetType().GetField(fieldName, bindFlags);
            return field.GetValue(instance) as TFieldType;
        }
    }
}
