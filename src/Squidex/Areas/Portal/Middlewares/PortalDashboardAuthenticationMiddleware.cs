﻿// ==========================================================================
//  LazyClientStore.cs
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex Group
//  All rights reserved.
// ==========================================================================

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;

namespace Squidex.Areas.Portal.Middlewares
{
    public sealed class PortalDashboardAuthenticationMiddleware
    {
        private readonly RequestDelegate next;

        public PortalDashboardAuthenticationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var authentication = await context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!authentication.Succeeded)
            {
                await context.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    RedirectUri = context.Request.PathBase + context.Request.Path
                });
            }
            else
            {
                await next(context);
            }
        }
    }
}
