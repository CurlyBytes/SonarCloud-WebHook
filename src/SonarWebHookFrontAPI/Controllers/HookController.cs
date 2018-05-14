using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using SonarCloudWebHookState.Interfaces;

namespace SonarWebHookFrontAPI.Controllers
{
    [Route("api/[controller]")]
    public class HookController : Controller
    {
        private readonly StatelessServiceContext _serviceContext;

        public HookController(StatelessServiceContext serviceContext)
        {
            _serviceContext = serviceContext;           
        }

        
        // GET api/hook/123456
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            string serviceUri = _serviceContext.CodePackageActivationContext.ApplicationName + "/SonarCloudWebHookStateActorService";
            ISonarCloudWebHookState proxy = ActorProxy.Create<ISonarCloudWebHookState>(new ActorId("State"), new Uri(serviceUri));
            
            var state= await proxy.GetStateAsync(id, CancellationToken.None);
            if (state != null)
                return Content(state.ToString());

            return NotFound();
        }

        // POST api/hook
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] WebHookPayload value)
        { 
            if (value == null)
                return BadRequest();

            if (value.properties?.buildIdentifier == null)
                return BadRequest();

            string serviceUri = _serviceContext.CodePackageActivationContext.ApplicationName + "/SonarCloudWebHookStateActorService";

            ISonarCloudWebHookState proxy = ActorProxy.Create<ISonarCloudWebHookState>(new ActorId("State"), new Uri(serviceUri));
            await proxy.SetStateAsync(value.properties.buildIdentifier, "OK"== value.qualityGate?.status, CancellationToken.None);
            return Ok();
        }        
    }
}
