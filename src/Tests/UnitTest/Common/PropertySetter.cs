using System;
using System.Reflection;

namespace UnitTest.Common
{
    public static class PropertySetter
    {
        public static void Set(object target,
                               string propertyName,
                               object propertyValue)
        {
            Type type = target.GetType();
            PropertyInfo prop = type.GetProperty(propertyName);
            prop.SetValue(target, propertyValue, null);
        }
    }
}
