using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationCore.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            const string Issuer = "https://contoso.com";
            var claims = new List<Claim>();
            claims.AddRange(
                new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "barry", ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.Role, "Administrator",ClaimValueTypes.String, Issuer),
                    new Claim("EmployeeId", "123", ClaimValueTypes.String, Issuer),
                    new Claim("BadgeNumber", "123456", ClaimValueTypes.String, Issuer),
                    new Claim(ClaimTypes.DateOfBirth, "1970-06-08", ClaimValueTypes.Date),
                    new Claim("TemporaryBadgeExpiry", 
                        DateTime.Now.AddDays(1).ToString(), 
                        ClaimValueTypes.String, 
                        Issuer)
                }
            );

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                #region OptionsHelpRegion

                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.

                #endregion

                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = false,
                AllowRefresh = false
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}