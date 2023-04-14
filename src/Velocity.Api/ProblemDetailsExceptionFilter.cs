using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Velocity.Api;

public class ProblemDetailsExceptionFilter: IExceptionFilter
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly ILogger<ProblemDetailsExceptionFilter> _logger;

    public ProblemDetailsExceptionFilter(ILogger<ProblemDetailsExceptionFilter> logger, ProblemDetailsFactory problemDetailsFactory)
    {
        _logger = logger;
        _problemDetailsFactory = problemDetailsFactory;
    }

    public void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails;
        switch (context.Exception)
        {
            case InvalidModelException exception:
                problemDetails = _problemDetailsFactory.CreateProblemDetails(
                    context.HttpContext,
                    StatusCodes.Status400BadRequest,
                    title: "Bad request",
                    detail: "Some of requested transactions have invalid format");
                break;
            default:
                _logger.LogError(context.Exception, "Unhandled error");
                problemDetails = _problemDetailsFactory.CreateProblemDetails(context.HttpContext,
                    500,
                    "Internal error",
                    detail: "Internal error occured. Please contact team");
                break;
        }
        
        context.Result = new ObjectResult(problemDetails);
        context.ExceptionHandled = true;
    }
}