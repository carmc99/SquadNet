// <copyright company="Carmc99 - SquadNet">
// Licensed under the Business Source License 1.0 (BSL 1.0)
// </copyright>
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SquadNET.Extensions.Exceptions.Exceptions;
using System.Net;
using System.Text.Json;

namespace SquadNET.Extensions.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> Logger;
        private readonly RequestDelegate Next;

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