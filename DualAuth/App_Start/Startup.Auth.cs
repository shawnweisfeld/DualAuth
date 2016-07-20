using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DualAuth
{
	public partial class Startup
	{

        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];

        private static string clientIdGov = ConfigurationManager.AppSettings["ida:ClientIdGov"];
        private static string aadInstanceGov = ConfigurationManager.AppSettings["ida:AADInstanceGov"];
        private static string tenantIdGov = ConfigurationManager.AppSettings["ida:TenantIdGov"];

        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            //Single Tenant (only the users in your AAD can authenticate)
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = aadInstance + tenantId,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    AuthenticationType = "AADC"
                });

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientIdGov,
                    Authority = aadInstanceGov + tenantIdGov,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    AuthenticationType = "AADG"
                });


            //Multi Tenant (users in any AAD can authenticate)
            //NOTE: you still need the Gov and Commercial configurations since the AAD instace for each is different
            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        ClientId = clientId,
            //        Authority = $"{aadInstanceGov}common",
            //        PostLogoutRedirectUri = postLogoutRedirectUri,
            //        AuthenticationType = "AADC",
            //        TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
            //            // we inject our own multitenant validation logic
            //            ValidateIssuer = false,
            //        },
            //    });

            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        ClientId = clientIdGov,
            //        Authority = $"{aadInstanceGov}common",
            //        PostLogoutRedirectUri = postLogoutRedirectUri,
            //        AuthenticationType = "AADG",
            //        TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            // instead of using the default validation (validating against a single issuer value, as we do in line of business apps), 
            //            // we inject our own multitenant validation logic
            //            ValidateIssuer = false,
            //        },
            //    });
        }
    }
}