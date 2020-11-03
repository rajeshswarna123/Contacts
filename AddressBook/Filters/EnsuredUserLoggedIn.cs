using AddressBook.Core.Common.Core;
using AddressBook.Core.Common.JwtHelper;
using AddressBook.Core.Models;
using AddressBook.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace AddressBook.API.Filters
{
    public class EnsureUserLoggedIn : ActionFilterAttribute
    {
        private readonly IUserContext _userContext;

        private readonly IHttpContextAccessor _httpContext;

        private readonly IUserService _userService;

        private readonly JwtIssuerOptions _jwtIssuerOptions;


        public EnsureUserLoggedIn(
            IUserContext userContext,
            IHttpContextAccessor httpContext,
            IUserService userService,
            JwtIssuerOptions jwtIssuerOptions)
        {
            this._userContext = userContext;
            this._httpContext = httpContext;
            this._userService = userService;
            this._jwtIssuerOptions = jwtIssuerOptions;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userId = this._httpContext.HttpContext.User.FindFirst("UserId")?.Value;
            DateTime? tokenExpiry = null;
            try
            {
                tokenExpiry = Convert.ToDateTime(this._httpContext.HttpContext.User.FindFirst("ExpiresOn")?.Value);
            }
            catch (Exception)
            {
                // throw;
            }

            string sessionId = this._httpContext.HttpContext.User.FindFirst("SessionId")?.Value;

            if (String.IsNullOrEmpty(userId))
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult(HttpStatusCode.Unauthorized, "Unauthorized");
                return;
            }


            User profile = this._userService.GetUserById(userId);

            this._userContext.UserInfo = profile;
            this._userContext.SessionId = sessionId;


            if (tokenExpiry != null)
            {
                this._userContext.TokenExpiry = tokenExpiry;
            }

        }

        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            if (this._userContext.TokenExpiry != null)
            {
                DateTime tokenExpiresOn = Convert.ToDateTime(this._userContext.TokenExpiry);
                if (DateTime.Now.AddMinutes(15) > tokenExpiresOn)
                {
                    // Generate new token
                    try
                    {
                        if (string.IsNullOrEmpty(actionExecutedContext.HttpContext.Response.Headers["RefreshToken"]))
                        {
                            var token = JwtTokenHelper.GenerateJSONWebToken(
                                this._jwtIssuerOptions,
                                this._userContext.UserInfo.Id,
                                this._userContext.UserInfo.Email,
                                this._userContext.SessionId);
                            actionExecutedContext.HttpContext.Response.Headers.Add("RefreshToken", token);
                        }
                    }
                    catch (Exception)
                    {
                        // throw;
                    }
                }
            }
        }
    }
}
