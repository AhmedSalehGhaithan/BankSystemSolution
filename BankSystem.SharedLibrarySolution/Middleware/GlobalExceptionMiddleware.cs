using BankSystem.SharedLibrarySolution.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BankSystem.SharedLibrarySolution.Middleware
{
    public class GlobalExceptionMiddleware(RequestDelegate _next)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            string title = "Error";
            string message = "An unexpected error occurred.";
            int statusCode = (int)HttpStatusCode.InternalServerError;

            try
            {
                await _next(context);
                statusCode = context.Response.StatusCode;

                // Handle specific status codes
                (title, message) = statusCode switch
                {
                    StatusCodes.Status400BadRequest =>
                        ("Bad Request", "The request was invalid or cannot be served."),
                    StatusCodes.Status401Unauthorized =>
                        ("Unauthorized", "You are not authorized to access this resource."),
                    StatusCodes.Status403Forbidden =>
                        ("Forbidden", "Access to this resource is forbidden."),
                    StatusCodes.Status404NotFound =>
                        ("Not Found", "The requested resource was not found."),
                    StatusCodes.Status409Conflict =>
                        ("Conflict", "The request could not be completed due to a conflict."),
                    StatusCodes.Status429TooManyRequests =>
                        ("Too Many Requests", "You have made too many requests in a given amount of time."),
                    StatusCodes.Status500InternalServerError =>
                        ("Internal Server Error", "An unexpected error occurred."),
                    StatusCodes.Status408RequestTimeout =>
                        ("Request Timeout", "The request took too long to complete."),
                    _ => (title, message)
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                (title, message, statusCode) = ex switch
                {
                    TaskCanceledException _ or TimeoutException _ =>
                        ("Request Timeout", "The request took too long to complete.", StatusCodes.Status408RequestTimeout),
                    _ => (title, message, statusCode)
                };
            }

            await WriteResponseAsync(context, title, message, statusCode);
        }

        private static async Task WriteResponseAsync(HttpContext context, string title, string message, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Detail = message,
                Status = statusCode
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails), CancellationToken.None);
        }
    }
}
