using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Velocity.Api.Models;

public class UpdateTransactionsRequest
{
    /// <summary>
    /// Requested File
    /// </summary>
    [Required]
    [FromForm(Name = "file")]
    public IFormFile File { get; set; }
}

public class UpdateTransactionsRequestValidator : AbstractValidator<UpdateTransactionsRequest>
{
    public UpdateTransactionsRequestValidator()
    {
        RuleFor(x => x.File).NotEmpty();
        RuleFor(request => request.File.Length).GreaterThan(0);
        RuleFor(x => x.File.ContentType).Must(type => type == "text/txt");
    }
}