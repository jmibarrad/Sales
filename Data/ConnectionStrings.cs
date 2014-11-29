using System.Configuration;

namespace Data
{
    public class ConnectionStrings
    {
        public static string Get()
        {
            
            var environment = (ConfigurationManager.AppSettings["Environment"] ?? "").ToLower();
            var connectionStringToUse = string.Empty;
            
            
            if (environment == "remote")
            {
                connectionStringToUse = ConfigurationManager.ConnectionStrings["remote"].ToString();
            }
            else if (environment == "local")
            {
                connectionStringToUse = ConfigurationManager.ConnectionStrings["local"].ToString();
            }

            return connectionStringToUse;
        }
    }
}