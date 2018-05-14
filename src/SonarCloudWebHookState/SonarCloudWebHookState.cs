using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using SonarCloudWebHookState.Interfaces;

namespace SonarCloudWebHookState
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class SonarCloudWebHookState : Actor, ISonarCloudWebHookState
    {
        /// <summary>
        /// Initializes a new instance of SonarCloudWebHookState
        /// </summary>
        /// <param name="actorService">The Microsoft.ServiceFabric.Actors.Runtime.ActorService that will host this actor instance.</param>
        /// <param name="actorId">The Microsoft.ServiceFabric.Actors.ActorId for this actor instance.</param>
        public SonarCloudWebHookState(ActorService actorService, ActorId actorId)
            : base(actorService, actorId)
        {
        }


        async Task<bool?> ISonarCloudWebHookState.GetStateAsync(string id, CancellationToken cancellationToken)
        {
            var state = await StateManager.TryGetStateAsync<bool>(id, cancellationToken);
            if (state.HasValue)
                return state.Value;

            return null;
        }
      
        async Task ISonarCloudWebHookState.SetStateAsync(string id, bool state, CancellationToken cancellationToken)
        {
            await StateManager.SetStateAsync(id, state, cancellationToken);
        }
    }
}
