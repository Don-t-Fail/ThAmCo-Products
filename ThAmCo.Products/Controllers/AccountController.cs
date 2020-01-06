using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.Products.Models;

namespace ThAmCo.Products.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            var client = GetHttpClient("StandardRequest");

            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:43389");
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "my_web_app",
                ClientSecret = "secret",

                UserName = model.Email,
                Password = model.Password
            });

            if (tokenResponse.IsError)
                return View();

            var userInfoResponse = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = tokenResponse.AccessToken
            });

            if (userInfoResponse.IsError)
                return View();

            var claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var tokensToStore = new AuthenticationToken[]
            {
                new AuthenticationToken { Name = "access_token", Value = tokenResponse.AccessToken }
            };
            var authProperties = new AuthenticationProperties();
            authProperties.StoreTokens(tokensToStore);

            await HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties);

            return LocalRedirect("/");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            return Ok("Signed Out");
        }

        [Authorize]
        public IActionResult Authed()
        {
            return Ok("Authed");
        }

        [Authorize(Policy = "StaffOnly")]
        public IActionResult StaffAuthed()
        {
            return Ok("Authed");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private HttpClient GetHttpClient(string s)
        {
            return _clientFactory.CreateClient(s);
        }
    }
}