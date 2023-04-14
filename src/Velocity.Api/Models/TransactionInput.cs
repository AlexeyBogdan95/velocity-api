using System;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Velocity.Application;

namespace Velocity.Api.Models;

public class TransactionInput
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("customer_id")]
    public string CustomerId { get; set; }
    
    [JsonPropertyName("load_amount")]
    public string Amount { get; set; }
    
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    public Transaction ToApplication()
    {
        Regex regex = new Regex("^[$][1-9]\\d*[.][0-9]{2}");
        
        
        if (!int.TryParse(Id, out int id))
        {
            throw new InvalidModelException("Transaction id should be a numeric value");
        }

        if (!int.TryParse(CustomerId, out int customerId))
        {
            throw new InvalidModelException("Customer id should be a numeric value");   
        }

        if (!regex.IsMatch(Amount))
        {
            throw new InvalidModelException("Amount has invalid format");
        }

        return new Transaction
        {
            Id = id,
            CustomerId = customerId,
            Amount = (int) (float.Parse(Amount.Substring(1), CultureInfo.InvariantCulture.NumberFormat) * 100),
            Time = Time
        };
    }
}