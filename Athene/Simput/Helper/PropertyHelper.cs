using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Simput.Helper
{
	public static class PropertyHelper
	{
		/// <summary>
		/// Extracs a property info from a given expression
		/// </summary>
		/// <typeparam name="TObjectType"></typeparam>
		/// <typeparam name="TMemberType"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		public static PropertyInfo GetPropertyInfoByExpression<TObjectType, TMemberType>(Expression<Func<TObjectType, TMemberType>> expression)
		{
			var context = (MemberExpression)expression.Body;

			return (PropertyInfo)context.Member;
		}
	}
}
