using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Globalization;

namespace KubTest.WebApi
{
    public class CommandNameConstraint : IRouteConstraint
    {
        private readonly Regex _pattern;

        public CommandNameConstraint(string commandName)
        {
            if (string.IsNullOrWhiteSpace(commandName))
                throw new ArgumentNullException(nameof(commandName));

            _pattern = new Regex(";command=" + commandName, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (route == null)
                throw new ArgumentNullException(nameof(route));

            if (routeKey == null)
                throw new ArgumentNullException(nameof(routeKey));

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            if (httpContext.Request.Method != HttpMethods.Post)
                return false;

            if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !_pattern.IsMatch(httpContext.Request.ContentType))
                return false;

            object value;
            if (values.TryGetValue(routeKey, out value) && value != null)
            {
                if (value is Guid)
                {
                    return true;
                }

                Guid result;
                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                return Guid.TryParse(valueString, out result);
            }

            return false;
        }
    }
}
