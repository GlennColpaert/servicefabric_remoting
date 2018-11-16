using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceFabricRemotingTest.MyServiceImplementation
{
    public class MyService : IMyService
    {
        public Task DeleteActorAsync(ActorId actorId, CancellationToken cancellationToken)
        { return Task.CompletedTask; }

        public Task<PagedResult<ActorInformation>> GetActorsAsync(ContinuationToken continuationToken, CancellationToken cancellationToken)
        { return Task.FromResult(new PagedResult<ActorInformation>()); }
    }
}
