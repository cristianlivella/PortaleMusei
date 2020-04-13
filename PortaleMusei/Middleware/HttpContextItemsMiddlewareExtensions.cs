using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortaleMusei.Middleware
{
    public static class HttpContextItemsMiddlewareExtensions
    {
        public static IApplicationBuilder
            UseHttpContextItemsMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpContextItemsMiddleware>();
        }
    }
}
