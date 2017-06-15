using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Ads
{
   /// <summary>
   /// Web API configuration class.
   /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register HTTP configuration.
        /// </summary>
        /// <param name="config">HttpConfiguration object</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Add filter for Basic Authentication
            config.Filters.Add(new BasicAuthenticationAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
