using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;

namespace quartznet_database_demo
{
    public static class Utils
    {
        public static NameValueCollection ParseQuartzConfig(IConfiguration config)
        {
            var properties = new NameValueCollection();
            var quartzConfig = config.GetSection("Quartz");

            foreach (var prop in quartzConfig.GetChildren())
            {
                properties[prop.Key.Trim()] = prop.Value.Trim();
            }
            
            return properties;
        }
    }
}
