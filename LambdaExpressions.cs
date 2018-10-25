using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        private void Testing()
        {
            String[] list;

            list = new String[] { "a", "b", "c", "ac", "ab", "cc", "d", "dd", "dc" };

            Expression<Func<String, Boolean>> stringLikeA = currentString => currentString.Contains("a");
            Expression<Func<String, Boolean>> stringLikeB = currentString => currentString.Contains("b");
            Expression<Func<String, Boolean>> stringLikeC = currentString => currentString.Contains("c");

            Expression<Func<String, Boolean>> neededUser = And<String>(stringLikeA, stringLikeB);
            list.Where(neededUser.Compile());

            //a
            Assert.IsTrue(list.Where(neededUser.Compile()).Count() == 1);  //ab

            //a, c, ac, ab, cc, dc
            neededUser = Or<String>(stringLikeA, stringLikeC);

            Assert.IsTrue(list.Where(neededUser.Compile()).Count() == 6);

            //ab, c, ac, cc, dc
            neededUser = And<String>(stringLikeA, stringLikeB);
            neededUser = Or<String>(neededUser, stringLikeC);
            Assert.IsTrue(list.Where(neededUser.Compile()).Count() == 5);
        }
    }
}
