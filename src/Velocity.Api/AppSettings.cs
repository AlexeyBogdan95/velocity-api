using Microsoft.Extensions.Configuration;

namespace Velocity.Api;

public class AppSettings
{
    public AppSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }
    
    public string ConnectionString { get; set; }
}
