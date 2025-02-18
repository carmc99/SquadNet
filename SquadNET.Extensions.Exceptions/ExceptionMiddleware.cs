using SquadNET.Extensions.Exceptions.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SquadNET.Extensions.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly ILogger<ExceptionMiddleware> Logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            Next = next;
            Logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (RconException rconEx)
            {
                Logger.LogError($"Error RCON ({rconEx.Code}): {rconEx.Message}");
                await HandleExceptionAsync(context, rconEx.Code, rconEx.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error inesperado: {ex.Message}");
                await HandleExceptionAsync(context, ErrorCode.UnknownError, ex.Message);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, ErrorCode code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                ErrorCode = code,
                Message = message
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
