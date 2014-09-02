using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property, string order = "asc")
        {
            return ApplyOrder(source, property, order == "asc" ? "OrderBy" : "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property,
            string order = "asc")
        {
            return ApplyOrder(source, property, order == "asc" ? "ThenBy" : "ThenByDescending");
        }

        private static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type[] type = {typeof (T)};
            ParameterExpression arg = Expression.Parameter(type[0], "x");
            Expression expr = arg;
            foreach (PropertyInfo pi in props.Select(prop => type[0].GetProperty(prop)))
            {
                expr = Expression.Property(expr, pi);
                type[0] = pi.PropertyType;
            }
            Type delegateType = typeof (Func<,>).MakeGenericType(typeof (T), type[0]);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof (Queryable).GetMethods().Single(
                method => method.Name == methodName
                          && method.IsGenericMethodDefinition
                          && method.GetGenericArguments().Length == 2
                          && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof (T), type[0])
                .Invoke(null, new object[] {source, lambda});
            return (IOrderedQueryable<T>) result;
        }
    }
}