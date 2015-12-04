using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Zkly.Common.Utils
{
    public delegate T ObjectActivator<out T>(params object[] args);

    public delegate T Method<out T>(object source, params object[] args);

    public delegate TValue PropertyGetter<out TValue>(object source);

    public delegate object PropertyGetter(object source);

    public delegate void PropertySetter(object source, object value);

    public static class ReflectionUtil
    {
        #region ObjectActivator
        public static ObjectActivator<T> GetDefaultActivator<T>()
        {
            return GetDefaultActivator<T>(typeof(T));
        }

        public static ObjectActivator<T> GetDefaultActivator<T>(this Type type)
        {
            var ctor = type.GetConstructors().OrderBy(c => c.GetParameters().Length).FirstOrDefault();
            return ctor == null ? null : GetActivator<T>(ctor);
        }

        public static ObjectActivator<T> GetActivator<T>(this Type type, params Type[] parameterTypes)
        {
            var ctor = type.GetConstructor(parameterTypes);
            return ctor == null ? null : GetActivator<T>(ctor);
        }

        public static ObjectActivator<T> GetActivator<T>(this ConstructorInfo ctor)
        {
            var paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            var param = Expression.Parameter(typeof(object[]), "args");
            var argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;
                var paramAccessorExp = Expression.ArrayIndex(param, index);
                var paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the ctor with the args we just created
            var body = Expression.New(ctor, argsExp);

            //create a lambda with the New Expression as body and our param as arg, and compile it
            return Expression.Lambda<ObjectActivator<T>>(body, param).Compile();
        }
        #endregion

        #region Object Method
        public static Method<T> GetMethod<T>(this Type type, string methodName, params Type[] parameterTypes)
        {
            var method = type.GetMethod(methodName, parameterTypes);
            return method == null ? null : GetMethod<T>(method);
        }

        public static Method<T> GetMethod<T>(this MethodInfo method)
        {
            var objParameter = Expression.Parameter(typeof(object), "x");
            Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
            var objCastParameter = Expression.Convert(objParameter, method.DeclaringType);

            var paramsInfo = method.GetParameters();

            //create a single param of type object[]
            var param = Expression.Parameter(typeof(object[]), "args");
            var argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;
                var paramAccessorExp = Expression.ArrayIndex(param, index);
                var paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the method with the args we just created
            var body = Expression.Call(objCastParameter, method, argsExp);

            //create a lambda with the New Expression as body and our param as arg, and compile it
            return Expression.Lambda<Method<T>>(body, objParameter, param).Compile();
        }
        #endregion

        #region Get Property Value
        public static PropertyGetter<TValue> GetPropertyGetter<TObject, TValue>(string propertyName)
        {
            return typeof(TObject).GetPropertyGetter<TValue>(propertyName);
        }

        public static PropertyGetter<T> GetPropertyGetter<T>(this Type type, string propertyName)
        {
            var objParameter = Expression.Parameter(typeof(object), "x");
            var objCastParameter = Expression.Convert(objParameter, type);
            var body = Expression.Property(objCastParameter, propertyName);

            //create a lambda with the New Expression as body and our param as arg, and compile it
            return Expression.Lambda<PropertyGetter<T>>(body, objParameter).Compile();
        }

        public static PropertyGetter<T> GetPropertyGetter<T>(this PropertyInfo propertyInfo)
        {
            var objParameter = Expression.Parameter(typeof(object), "x");
            Debug.Assert(propertyInfo.DeclaringType != null, "propertyInfo.DeclaringType != null");
            var objCastParameter = Expression.Convert(objParameter, propertyInfo.DeclaringType);
            var body = Expression.Property(objCastParameter, propertyInfo);
            Expression converted = Expression.Convert(body, typeof(T));

            //create a lambda with the New Expression as body and our param as arg, and compile it
            return Expression.Lambda<PropertyGetter<T>>(converted, objParameter).Compile();
        }
        #endregion

        #region Set Property Value
        public static PropertySetter GetPropertySetter<T>(string propertyName)
        {
            return typeof(T).GetPropertySetter(propertyName);
        }

        public static PropertySetter GetPropertySetter(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName).GetPropertySetter();
        }

        public static PropertySetter GetPropertySetter(this PropertyInfo propertyInfo)
        {
            var setterMethod = propertyInfo.GetSetMethod(true);

            var objParameter = Expression.Parameter(typeof(object), string.Empty);
            var valueParameter = Expression.Parameter(typeof(object), string.Empty);

            Debug.Assert(propertyInfo.DeclaringType != null, "propertyInfo.DeclaringType != null");
            var objCastParameter = Expression.Convert(objParameter, propertyInfo.DeclaringType);
            var valueCastParameter = Expression.Convert(valueParameter, propertyInfo.PropertyType);

            var body = Expression.Call(objCastParameter, setterMethod, valueCastParameter);

            //create a lambda with the New Expression as body and our param as arg, and compile it
            return Expression.Lambda<PropertySetter>(body, objParameter, valueParameter).Compile();
        }
        #endregion

        /// <summary>
        /// Get Type from current assemblies
        /// </summary>
        /// <param name="typeFullName">type asembly qualified name</param>
        /// <returns>Type</returns>
        public static Type GetType(string typeFullName)
        {
            var type = Type.GetType(typeFullName);
            if (type != null)
            {
                return type;
            }

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeFullName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
