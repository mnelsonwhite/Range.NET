using Range.Net.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Range.Net
{
    public static class RangeQueryableExtensions
    {
        /// <summary>
        /// Filter an IQueryable property by range using range inclusivity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TProperty">Entity property type</typeparam>
        /// <param name="queryable">queryable to filter</param>
        /// <param name="property">property of queryable to filter by</param>
        /// <param name="range">The range type must be the same as the entity property type</param>
        /// <returns>Queryable with property filtered by range</returns>
        public static IQueryable<TEntity> FilterByRange<TEntity, TProperty>(
            this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty>> property,
            IRange<TProperty> range)
            where TProperty : IComparable<TProperty>
        {
            return queryable.Where(FilterExpressionByRange(queryable, property, range));
        }

        /// <summary>
        /// Filter an IQueryable property by range using range inclusivity
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TProperty">Entity property type</typeparam>
        /// <param name="queryable">queryable to filter</param>
        /// <param name="property">nullable property of queryable to filter by</param>
        /// <param name="range">The range type must be the same as the entity property type</param>
        /// <returns>Queryable with property value filtered by range</returns>
        public static IQueryable<TEntity> FilterByRange<TEntity, TProperty>(
            this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty?>> property,
            IRange<TProperty> range)
            where TProperty : struct, IComparable<TProperty>
        {
            return queryable.Where(FilterExpressionByRange(queryable, property, range));
        }

        public static Expression<Func<TEntity, bool>> FilterExpressionByRange<TEntity, TProperty>(
            this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty>> property,
            IRange<TProperty> range)
            where TProperty : IComparable<TProperty>
        {
            var paramExpr = Expression.Parameter(typeof(TEntity), "e");
            var memberExpr = (MemberExpression) property.Body;
            var propertyExpr = Expression.MakeMemberAccess(paramExpr, memberExpr.Member);

            var body = GetBinaryExpression(range, propertyExpr);
            var filterExpression = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);

            return filterExpression;
        }

        public static Expression<Func<TEntity, bool>> FilterExpressionByRange<TEntity, TProperty>(
            this IQueryable<TEntity> queryable,
            Expression<Func<TEntity, TProperty?>> property,
            IRange<TProperty> range)
            where TProperty : struct, IComparable<TProperty>
        {
            var paramExpr = Expression.Parameter(typeof(TEntity), "e");
            var nullableExpr = Expression.MakeMemberAccess(
                paramExpr,
                ((MemberExpression)property.Body).Member
            );
            var valueMember = typeof(TProperty?).GetMember("Value").First();
            var propertyExpr = Expression.MakeMemberAccess(nullableExpr, valueMember);

            var body = GetBinaryExpression(range, propertyExpr);
            var filterExpression = Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);

            return filterExpression;
        }

        public static BinaryExpression GetBinaryExpression<TProperty>(
            IRange<TProperty> range,
            MemberExpression propertyExpression)
            where TProperty : IComparable<TProperty>
        {
            var minConst = Expression.Constant(range.Minimum);
            var maxConst = Expression.Constant(range.Maximum);
            var lhs = Expression.MakeBinary(
                range.Inclusivity == RangeInclusivity.ExclusiveMinExclusiveMax ||
                range.Inclusivity == RangeInclusivity.ExclusiveMinInclusiveMax
                    ? ExpressionType.GreaterThan
                    : ExpressionType.GreaterThanOrEqual,
                GetNullableValue(propertyExpression),
                minConst);
            var rhs = Expression.MakeBinary(
                range.Inclusivity == RangeInclusivity.ExclusiveMinExclusiveMax ||
                range.Inclusivity == RangeInclusivity.InclusiveMinExclusiveMax
                    ? ExpressionType.LessThan
                    : ExpressionType.LessThanOrEqual,
                GetNullableValue(propertyExpression),
                maxConst);
            var body = Expression.MakeBinary(ExpressionType.And, lhs, rhs);

            return body;
        }

        private static MemberExpression GetNullableValue(
            MemberExpression memberExpr)
        {
            var t = typeof(DateTime?);
            if (memberExpr.Type != t) return memberExpr;

            var valMember = t.GetProperty("Value");
            if (valMember == null) throw new InvalidOperationException("Value property");
            return Expression.MakeMemberAccess(memberExpr, valMember);
        }
    }
}
