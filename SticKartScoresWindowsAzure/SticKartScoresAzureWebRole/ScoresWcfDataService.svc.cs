using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace SticKartScoresAzureWebRole
{
    public class ScoresWcfDataService : DataService<SticKartScores_0Entities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            int numTables = 11;
            for (int count = 0; count < numTables; ++count)
            {
                int padding = 3 - count.ToString().Length;
                string entity = "HighScores_" + new string('0', padding) + count.ToString();
                config.SetEntitySetAccessRule(entity, EntitySetRights.All);
            }

            config.UseVerboseErrors = false;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
        }
    }
}
