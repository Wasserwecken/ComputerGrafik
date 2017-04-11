using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Simput.Helper
{
	/// <summary>
	/// Helps to work with properties
	/// </summary>
	internal static class PropertyHelper
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
			var context = expression.Body as MemberExpression;
			if (context == null)
			{
				var op = ((UnaryExpression)expression.Body).Operand;
				context = (MemberExpression)op;
			}

			return (PropertyInfo)context.Member;
		}
	}
}
