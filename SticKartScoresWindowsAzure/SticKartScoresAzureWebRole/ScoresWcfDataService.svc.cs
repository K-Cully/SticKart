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
            // TODO: Create access methods and filter by level input
            config.SetEntitySetAccessRule("HighScores_000", EntitySetRights.AllRead);

            config.UseVerboseErrors = false;
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
        }
    }
}
