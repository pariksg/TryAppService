﻿using System.Web;
using System.Collections.Generic;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using SimpleWAWS.Trace;
using System.Threading.Tasks;
using System.Globalization;
using System.Linq;

namespace SimpleWAWS.Authentication
{
    public static class SecurityManager
    {
        private static readonly Dictionary<string, IAuthProvider> _authProviders =
            new Dictionary<string, IAuthProvider>(StringComparer.OrdinalIgnoreCase);
        private static IEnumerable<BaseOpenIdConnectAuthProvider> _openIdAuthProviders;

        public static string SelectedProvider(HttpContextBase context)
        {
            if (!string.IsNullOrEmpty(context.Request.QueryString["provider"]))
                return context.Request.QueryString["provider"];

            var state = context.Request.QueryString["state"];
            if (string.IsNullOrEmpty(state))
                return AuthConstants.DefaultAuthProvider;

            state = WebUtility.UrlDecode(state);
            var match = Regex.Match(state, "provider=([^&]+)", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups[1].Value : AuthConstants.DefaultAuthProvider;
        }

        private static IAuthProvider GetAuthProvider(HttpContextBase context)
        {
            var requestedAuthProvider = SelectedProvider(context);

            IAuthProvider authProvider;
            if (_authProviders.TryGetValue(requestedAuthProvider, out authProvider))
            {
                return authProvider;
            }
            else
            {
                return _authProviders[AuthConstants.DefaultAuthProvider];
            }
        }

        public static void InitAuthProviders()
        {
            _authProviders.Add("AAD", new AADProvider());
            _authProviders.Add("ReCaptcha", new ReCaptchaAuthProvider());
            _authProviders.Add("Facebook", new FacebookAuthProvider());
            _authProviders.Add("Twitter", new TwitterAuthProvider());
            _authProviders.Add("Google", new GoogleAuthProvider());
            _authProviders.Add("Vk", new VkAuthProvider());
            _authProviders.Add("GitHub", new GitHubAuthProvider());
            _openIdAuthProviders = _authProviders
                    .Where(e => e.Value is BaseOpenIdConnectAuthProvider)
                    .Select(s => s.Value)
                    .Cast<BaseOpenIdConnectAuthProvider>();
        }

        public static void AuthenticateRequest(HttpContextBase context)
        {
            GetAuthProvider(context).AuthenticateRequest(context);
        }

        public static bool HasToken(HttpContextBase context)
        {
            return GetAuthProvider(context).HasToken(context);
        }

        public static bool IsAdmin(HttpContextBase context)
        {
            return AuthSettings.AdminUserKeys.Split(';').Any(n => n == context.Request.Params["Key"]);
        }

        public static bool TryAuthenticateSessionCookie(HttpContextBase context)
        {
            try
            {
                var authHeader = context.Request.Headers["Authorization"];
                var token = string.Empty;
                if (authHeader != null &&
                    authHeader.IndexOf(AuthConstants.BearerHeader, StringComparison.OrdinalIgnoreCase) > -1)
                    token = authHeader.Substring(AuthConstants.BearerHeader.Length).Trim();

                var isrecaptcha = context.Request.QueryString["provider"]?.Equals("recaptcha", StringComparison.OrdinalIgnoreCase)??false || token.Length>1000;

                var loginSessionCookie = (isrecaptcha)? 
                    Crypto.DecryptStringAES((token))
                        : Uri.UnescapeDataString(token);

                var splited = loginSessionCookie.Split(';');

                if (splited.Length == 2)
                {
                    var user = splited[0];
                    var date = DateTime.Parse(splited[1], CultureInfo.InvariantCulture);
                    if (ValidDateTimeSessionCookie(date))
                    {
                        context.User = new TryWebsitesPrincipal(new TryWebsitesIdentity(user, user, "Old"));
                        return true;
                    }
                }
                else if (splited.Length == 4)
                {
                    var date = DateTime.Parse(loginSessionCookie.Split(';')[3], CultureInfo.InvariantCulture);
                    if (ValidDateTimeSessionCookie(date))
                    {
                        var email = splited[0];
                        var puid = splited[1];
                        var issuer = splited[2];
                        context.User = new TryWebsitesPrincipal(new TryWebsitesIdentity(email, puid, issuer));
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                // we need to authenticate
            }
            catch (FormatException)
            {
                // Swallow and authenticate. This has been noisy lately,with a bulk of them coming from a single bad session
            }
            catch (Exception e)
            {
                // we need to authenticate
                //but also log the error
                SimpleTrace.Diagnostics.Error(e, "Exception during cookie authentication");
            }
            return false;
        }

        public static bool TryAuthenticateBearer(HttpContextBase context)
        {
            try
            {
                foreach (var authProvider in _openIdAuthProviders)
                {
                    if (authProvider.TryAuthenticateBearer(context) == TokenResults.ExistsAndCorrect)
                    {
                        return true;
                    }
                }
            }
            catch (NullReferenceException)
            {
                // we need to authenticate
            }
            catch (Exception e)
            {
                // we need to authenticate
                //but also log the error
                SimpleTrace.Diagnostics.Error(e, "Exception during bearer authentication");
            }
            return false;
        }

        public static void HandleAnonymousUser(HttpContextBase context)
        {
            try
            {
                if (!context.IsBrowserRequest()) return;
                var userCookie = context.Request.Cookies[AuthConstants.AnonymousUser];
                if (userCookie == null)
                {
                    var user = Guid.NewGuid().ToString();
                    context.Response.Cookies.Add(new HttpCookie(AuthConstants.AnonymousUser, Uri.EscapeDataString(Crypto.EncryptStringAES( user))) { Path = "/", Expires = DateTime.UtcNow.AddHours(1) });
                }
                else
                {
                    var user = Uri.UnescapeDataString(Crypto.DecryptStringAES(userCookie.Value));
                    context.User = new TryWebsitesPrincipal(new TryWebsitesIdentity(user, null, "Anonymous"));
                }
            }
            catch (Exception e)
            {
                SimpleTrace.Diagnostics.Error(e, "Error Adding anonymous user");
            }
        }

        public static string GetAnonymousUserName(HttpContextBase context)
        {
            try
            {
                var userCookie = context.Request.Cookies[AuthConstants.AnonymousUser];
                if (userCookie != null)
                {
                    var user = Uri.UnescapeDataString(Crypto.DecryptStringAES(userCookie.Value));
                    return new TryWebsitesIdentity(user, null, "Anonymous").Name;
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                SimpleTrace.Diagnostics.Error(e, "ERROR_GET_ANONYMOUS_USER");
                return string.Empty;
            }
        }

        public static HttpResponseMessage RedirectToAAD(string redirectContext)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            var context = new HttpContextWrapper(HttpContext.Current);
            response.Headers.Add("LoginUrl", (_authProviders["AAD"] as AADProvider).GetLoginUrl(context));

            if (context.Response.Cookies[AuthConstants.LoginSessionCookie] != null)
            {
                response.Headers.AddCookies(new [] { new CookieHeaderValue(AuthConstants.LoginSessionCookie, string.Empty){ Expires = DateTimeOffset.UtcNow.AddDays(-1), Path = "/" } });
            }
            return response;
        }

        public static Task<HttpResponseMessage> AdminOnly(Func<Task<HttpResponseMessage>> func)
        {
            if (SecurityManager.IsAdmin(new HttpContextWrapper(HttpContext.Current)))
            {
                return func();
            }
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden));
        }

        private static bool ValidDateTimeSessionCookie(DateTime date)
        {
            return date.Add(AuthConstants.SessionCookieValidTimeSpan) > DateTime.UtcNow;
        }
    }
}
