using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace NetSchoolWeb.Infrastructure
{
    public class DateConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            DateTime datetime;

            bool result = DateTime.TryParseExact(values[parameterName].ToString(),
            "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datetime);

            return result;
        }
    }
}