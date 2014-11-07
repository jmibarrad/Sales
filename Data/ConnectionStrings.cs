using System.Configuration;

namespace Data
{
    public class ConnectionStrings
    {
        public static string Get()
        {
            var local = ConfigurationManager.ConnectionStrings["local"].ToString();

            //var remote = (ConfigurationManager.ConnectionStrings["remote"].ToString());

            //var production = (ConfigurationManager.ConnectionStrings["production"].ToString());

            var environment = (ConfigurationManager.AppSettings["Environment"] ?? "").ToLower();
            var connectionStringToUse = local;


            if (environment == "qa" || environment == "remote")
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