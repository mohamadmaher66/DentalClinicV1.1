using Enums;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Request;
using System.Collections.Generic;

namespace DentalClinicAPI.Helpers
{
    public class ExceptionHandler
    {
        public static async void HandleException(HttpContext context)
        {
            var feature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = feature.Error;

            Logger.Log(exception);

            Alert alert = new Alert
            {
                Type = AlertTypeEnum.Error,
                Message = exception.Message,
                Title = "حذث خطأ"
            };

            if (exception.InnerException is SqlException sqlEx && (sqlEx.Number == 547))
            {
                alert.Message = "يجب حذف البيانات المربوطة اولا";
            }
            RequestedData<object> requestedData = new RequestedData<object>
            {
                Alerts = new List<Alert>() { alert }
            };

            var result = JsonConvert.SerializeObject(requestedData, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("access-control-allow-origin", "*");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(result);
        }
    }
}
