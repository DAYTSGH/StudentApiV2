﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApiV2.Utils
{
    public class OptionsRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public OptionsRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                return;
            }

            await _next.Invoke(context);
        }
    }
    public static class OptionsRequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseOptionsRequest(this IApplicationBuilder app)
        {
            return app.UseMiddleware<OptionsRequestMiddleware>();
        }
    }
}
