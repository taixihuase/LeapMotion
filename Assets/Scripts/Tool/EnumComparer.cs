using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tool
{
    public class EnumComparer<T> : IEqualityComparer<T> where T : struct
    {
        #region IEqualityComparer<T> 泛型接口实现

        public bool Equals(T first, T second)
        {
            var firstParam = Expression.Parameter(typeof(T), "first");
            var secondParam = Expression.Parameter(typeof(T), "second");
            var equalExpression = Expression.Equal(firstParam, secondParam);

            return Expression.Lambda<Func<T, T, bool>>
                (equalExpression, firstParam, secondParam).
                Compile().Invoke(first, second);
        }

        public int GetHashCode(T instance)
        {
            var parameter = Expression.Parameter(typeof(T), "instance");
            var convertExpression = Expression.Convert(parameter, typeof(int));

            return Expression.Lambda<Func<T, int>>
                (convertExpression, parameter).
                Compile().Invoke(instance);
        }

        #endregion
    }
}
