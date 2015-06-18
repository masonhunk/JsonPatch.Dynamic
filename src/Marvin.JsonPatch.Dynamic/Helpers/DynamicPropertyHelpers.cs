using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvin.JsonPatch.Dynamic.Helpers
{
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.Serialization;

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
            if (value.GetType().GetTypeInfo().ImplementedInterfaces.Any(i => i == typeof(IConvertible)))
            {
                return new ConversionResult(true,Convert.ChangeType(value,targetType));
            }
            ParameterExpression lambdaParam = Expression.Parameter(typeof(object));
            Expression cast = Expression.Convert(lambdaParam, targetType);
 
            
            var lambda = Expression.Lambda<Func<object, object>>(cast, lambdaParam);
            var convertMethod= lambda.Compile();
            
            try
            {
                var converted = convertMethod(value);
                return new ConversionResult(true,converted);
            }
            catch (InvalidCastException)
            {
                return new ConversionResult(false,null);
            }
            
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
