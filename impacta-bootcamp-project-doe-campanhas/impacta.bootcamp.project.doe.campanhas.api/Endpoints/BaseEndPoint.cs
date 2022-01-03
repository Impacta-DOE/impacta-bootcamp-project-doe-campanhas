using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints
{
    [ApiController]
    public abstract class BaseEndPoint<TRequest,TResponse>: BaseEndPoint
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
     
    }
    [ApiController]
    public abstract class BaseEndPoint< TResponse> : BaseEndPoint
    {
        public abstract Task<ActionResult<TResponse>> HandleAsync( CancellationToken cancellationToken = default);

    }
    //[Authorize]
    [ApiController]
    public abstract class BaseEndPoint : ControllerBase
    { }
}
