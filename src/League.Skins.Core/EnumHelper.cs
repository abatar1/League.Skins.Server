using System;
using System.Linq.Expressions;
using System.Reflection;

namespace League.Skins.Core
{
    public static class EnumHelper
    {
        public static void SetEnum<TResponse, TEnum>(TResponse model, string value,
            Expression<Func<TResponse, TEnum>> enumLambda, out ServiceResponse errorResponse)
            where TResponse : class
        {
            if (!(enumLambda.Body is MemberExpression memberSelectorExpression))
                throw new Exception("Wrong usage of SetEnum helper method.");

            var property = memberSelectorExpression.Member as PropertyInfo;
            if (property == null)
                throw new Exception("Wrong usage of SetEnum helper method.");

            var propertyType = property.PropertyType;
            var enumNullableType = Nullable.GetUnderlyingType(propertyType);
            if (enumNullableType != null && string.IsNullOrEmpty(value))
            {
                errorResponse = null;
                return;
            }

            if (!Enum.TryParse(enumNullableType ?? typeof(TEnum), value, out var result))
            {
                errorResponse = ServiceResponse.Error(ErrorServiceCodes.WrongEnumFormat,
                    $"Model [{typeof(TResponse).FullName}] has enum " +
                    $"[{(enumNullableType ?? typeof(TEnum)).Name}] with invalid value [{value}].");
                return;
            }

            errorResponse = null;
            property.SetValue(model, result, null);
        }
    }
}
