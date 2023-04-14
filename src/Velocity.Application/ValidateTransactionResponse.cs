namespace Velocity.Application;

public class ValidateTransactionResponse
{
    public TransactionOutput TransactionOutput { get; set; }
    public bool AlreadyExists { get; set; }

    private ValidateTransactionResponse() {}
    
    public static ValidateTransactionResponse Ok(TransactionOutput transactionOutput)
    {
        return new ValidateTransactionResponse
        {
            TransactionOutput = transactionOutput
        };
    }
    
    public static ValidateTransactionResponse MarkAsAlreadyExists()
    {
        return new ValidateTransactionResponse
        {
            AlreadyExists = true
        };
    }
}