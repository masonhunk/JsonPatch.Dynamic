using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvin.JsonPatch.Dynamic.Helpers
{
    using System.Reflection;

    internal class DynamicPropertyHelpers
    {
        public static Type GetEnumerableType(Type nominalType)
        {
            if (nominalType.IsArray)
            {
                return nominalType.GetElementType();
            }
            else
            {
                Type elementType;
                if (DynamicPropertyHelpers.TryGetElementType(nominalType, out elementType))
                {
                    return elementType;
                }
                else
                {
                    foreach (var @interface in nominalType.GetTypeInfo().ImplementedInterfaces)
                    {
                        if (DynamicPropertyHelpers.TryGetElementType(@interface, out elementType))
                        {
                            return elementType;
                        }
                    }
                }
                return typeof(object);


            }
        }

        public static ConversionResult ConvertToActualType(Type targetType,object value)
        {
            //TODO:
        }

        internal static bool TryGetElementType(Type nominalType,out Type elementType)
        {
            if (nominalType.GetGenericTypeDefinition() == typeof(IList<>))
            {
                elementType = nominalType.GenericTypeArguments[0];
                return true;
            }
            elementType = null;
            return false;
        }
    }
}
