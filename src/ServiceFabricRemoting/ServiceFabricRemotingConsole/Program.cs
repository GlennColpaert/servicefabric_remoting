using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Actors.Query;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

[assembly: FabricTransportActorRemotingProvider(RemotingListenerVersion = RemotingListenerVersion.V2_1, RemotingClientVersion = RemotingClientVersion.V2_1)]
namespace ServiceFabricRemotingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FabricClient fabricClient = new FabricClient("localhost:19000");
            ServicePartitionList partitionList = fabricClient.QueryManager.GetPartitionListAsync(new Uri("fabric:/Codit.Core.Application/DeviceActorService")).Result;

            ContinuationToken continuationToken = null;
            var cancellationTokenSource = new CancellationTokenSource();
            List<ActorInformation> activeActors = new List<ActorInformation>();

            foreach (var partition in partitionList)
            {
                var key = partition.PartitionInformation as Int64RangePartitionInformation;
                IActorService actorServiceProxy = ActorServiceProxy.Create(new Uri("fabric:/Codit.Core.Application/DeviceActorService"), key.LowKey);

                do
                {
                    PagedResult<ActorInformation> page = actorServiceProxy.GetActorsAsync(continuationToken, cancellationTokenSource.Token).Result;
                    activeActors.AddRange(page.Items.Where(x => x.IsActive));
                    continuationToken = page.ContinuationToken;
                }
                while (continuationToken != null);
            }

            foreach (var actor in activeActors)
            {
                Console.WriteLine($"Active Actor: {actor.ActorId.ToString()}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
