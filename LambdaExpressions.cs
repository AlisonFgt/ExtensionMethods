using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LambdaExpressions
{
    public class LambdaTest
    {
        public static Expression<Func<T, Boolean>> And<T>(
                        Expression<Func<T, Boolean>> expressionOne,
                        Expression<Func<T, Boolean>> expressionTwo)
        {
            var invokedSecond = Expression.Invoke(expressionTwo, expressionOne.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, Boolean>>(Expression.And(expressionOne.Body, invokedSecond), expressionOne.Parameters);
        }

        public static Expression<Func<T, Boolean>> Or<T>(
                        Expression<Func<T, Boolean>> expressionOne,
                        Expression<Func<T, Boolean>> expressionTwo)
        {
            var invokedSecond = Expression.Invoke(expressionTwo, expressionOne.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, Boolean>>(Expression.Or(expressionOne.Body, invokedSecond), expressionOne.Parameters);
        }
    }
}
