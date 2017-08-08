﻿using System.Configuration;
using System.Linq.Expressions;
using System.Net;
using Auth0.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(MvcApplication.Startup))]

namespace MvcApplication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Auth0 parameters
            string auth0Domain = ConfigurationManager.AppSettings["auth0:Domain"];
            string auth0ClientId = ConfigurationManager.AppSettings["auth0:ClientId"];
            string auth0ClientSecret = ConfigurationManager.AppSettings["auth0:ClientSecret"];

            // Enable Kentor Cookie Saver middleware
            app.UseKentorOwinCookieSaver();

            // Set Cookies as default authentication type
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login")
            });

            // Configure Auth0 authentication
            var options = new Auth0AuthenticationOptions()
            {
                Domain = auth0Domain,
                ClientId = auth0ClientId,
                ClientSecret = auth0ClientSecret,

                // If you want to save tokens, then please uncomment the code below
                // SaveAccessToken = true,

                // If you want to request an access_token to pass to an API, then uncomment the code below
                // Provider = new Auth0AuthenticationProvider
                // {
                //     OnApplyRedirect = context =>
                //     {
                //         context.RedirectUri += "&audience=" + WebUtility.UrlEncode("YOUR_API_IDENTIFIER");

                //         context.Response.Redirect(context.RedirectUri);
                //     }
                // }
            };
            options.Scope.Add("openid profile"); // Request a refresh_token
            app.UseAuth0Authentication(options);
        }
    }
}
