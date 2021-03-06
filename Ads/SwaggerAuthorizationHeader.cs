﻿using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Ads
{
    ///<Summary>
    /// SwaggerAuthorizationHeader to add Authorization header in swagger UI.
    ///</Summary>
    public class SwaggerAuthorizationHeader : IOperationFilter
    {
        /// <summary>
        /// Adds an authorization header to the given operation in Swagger.
        /// </summary>
        /// <param name="operation">The Swashbuckle operation.</param>
        /// <param name="schemaRegistry">The Swashbuckle schema registry.</param>
        /// <param name="apiDescription">The Swashbuckle api description.</param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation == null) return;

            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            var parameter = new Parameter
            {
                description = "Authorization Token",
                @in = "header",
                name = "Authorization",
                required = true,
                type = "string"
            };

            if (apiDescription.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                parameter.required = false;
            }

            operation.parameters.Add(parameter);
        }
    }
}