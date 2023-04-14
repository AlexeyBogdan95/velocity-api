using System.Text.Json.Serialization;

namespace Velocity.Api.Models;

public class TransactionOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("customer_id")]
    public string CustomerId { get; set; }
    
    [JsonPropertyName("accepted")]
    public bool Accepted { get; set; }


    public static TransactionOutput FromApplication(Application.TransactionOutput transactionOutput)
    {
        return new TransactionOutput
        {
            Id = transactionOutput.Id.ToString(),
            CustomerId = transactionOutput.CustomerId.ToString(),
            Accepted = transactionOutput.Accepted
        };
    }
}