using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velocity.Api.Models;
using Velocity.Application;

namespace Velocity.Api.Controller;

[ApiController]
public class TransactionsController: ControllerBase
{
    private readonly ITransactionsThresholdValidator _transactionsThresholdValidator;
    
    public TransactionsController(ITransactionsThresholdValidator transactionsThresholdValidator)
    {
        _transactionsThresholdValidator = transactionsThresholdValidator;
    }

    /// <summary>
    /// Upload transactions as a txt file
    /// </summary>
    /// <param name="request">Upload request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    [HttpPut("api/transactions.txt")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<FileContentResult> Update([FromForm] UpdateTransactionsRequest request, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(request.File.OpenReadStream());
        var content = await reader.ReadToEndAsync(cancellationToken);
        var items = content
            .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
            .ToArray();

        var transactionsToProcess = items
            .Select(x => JsonSerializer.Deserialize<TransactionInput>(x))
            .DistinctBy(x => new {x.Id, x.CustomerId})
            .ToArray();

        StringBuilder sb = new StringBuilder();
        foreach (var item in transactionsToProcess)
        {
            var response =
                await _transactionsThresholdValidator.Validate(item.ToApplication(), cancellationToken);
            if (response.AlreadyExists) 
                continue;

            sb.Append(JsonSerializer.Serialize(Models.TransactionOutput.FromApplication(response.TransactionOutput)));
            sb.Append("\r\n");
        }

        return new FileContentResult(Encoding.ASCII.GetBytes(sb.ToString()), "text/txt")
        {
            FileDownloadName = "output.txt"
        };
    }
}