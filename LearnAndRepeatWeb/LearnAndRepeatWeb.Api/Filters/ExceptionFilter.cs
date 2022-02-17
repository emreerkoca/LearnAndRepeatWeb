using LearnAndRepeatWeb.Business.CustomExceptions;
using LearnAndRepeatWeb.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace LearnAndRepeatWeb.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            if (exception is NotFoundException)
            {
                httpStatusCode = HttpStatusCode.NotFound;
            }
            else if (exception is ValidationException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
            }
            else if (exception is ConflictException)
            {
                httpStatusCode = HttpStatusCode.Conflict;
            }

            ErrorResponse errorResponse = new ErrorResponse
            {
                StatusCode = (int)httpStatusCode,
                Message = exception.Message
            };

            context.Result = new JsonResult(errorResponse);
        }
    }
}
