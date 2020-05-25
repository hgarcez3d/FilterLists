﻿using System;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

#pragma warning disable 1591

namespace FilterLists.Api.Odata
{
    public static class ServiceRegistration
    {
        public static void SetOdataOutputFormatters(this MvcOptions mvcOptions)
        {
            _ = mvcOptions ?? throw new ArgumentNullException(nameof(mvcOptions));

            var oDataOutputFormatters = mvcOptions.OutputFormatters.OfType<ODataOutputFormatter>()
                .Where(f => f.SupportedMediaTypes.Count == 0);
            var odataMediaTypeHeaderValue = new MediaTypeHeaderValue("application/odata");
            foreach (var oDataOutputFormatter in oDataOutputFormatters)
                oDataOutputFormatter.SupportedMediaTypes.Add(odataMediaTypeHeaderValue);
        }

        public static void ConfigureOdata(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MaxTop(10).Expand().Select().Filter().OrderBy().Count().SkipToken();
            endpointRouteBuilder.MapODataRoute("odata", default, EdmModelFactory.GetEdmModel());
        }
    }
}