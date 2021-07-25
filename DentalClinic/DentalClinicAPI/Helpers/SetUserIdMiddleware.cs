using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Request;

namespace DentalClinicAPI.Helpers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SetUserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public SetUserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Method == HttpMethods.Post
                    && !httpContext.Request.ContentType.Contains("multipart/form-data")
                    && httpContext.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    int userId = Convert.ToInt32(httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                    httpContext.Request.EnableBuffering();

                    using (var reader = new StreamReader(httpContext.Request.Body))
                    {
                        string requestBodyAsString = await reader.ReadToEndAsync();
                        RequestedData<object> objRequestBody = JsonConvert.DeserializeObject<RequestedData<object>>(requestBodyAsString);
                        objRequestBody.UserId = userId;
                        
                        var bodyAsJson = JsonConvert.SerializeObject(objRequestBody);
                        var requestContent = new StringContent(bodyAsJson, Encoding.UTF8, "application/json");
                        httpContext.Request.Body = await requestContent.ReadAsStreamAsync();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SetUserIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseSetUserIdMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetUserIdMiddleware>();
        }
    }
}
