using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RouteAPI.Contracts.V1.Requests;
using RouteAPI.Contracts.V1.Routes;
using RouteAPI.RabbitMQ.Producer;
using RouteAPI.RabbitMQ.Requests;
using RouteAPI.RabbitMQ.Responses;
using System.Linq;
using System.Threading.Tasks;

namespace RouteAPI.Controllers
{
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly ILogger<RouteController> _logger;
        private readonly IRouteAPIRabbitRPCService _routeAPIRabbitRPCService;

        private const string UserIdClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        private const string GetAllRoutesMethod = "GetAllRoutes";
        private const string GetRouteMethod = "GetRoute";
        private const string CreateRouteMethod = "CreateRoute";
        private const string DeleteRouteMethod = "DeleteRoute";
        private const string UpdateRouteMethod = "UpdateRoute";

        public RouteController(ILogger<RouteController> logger, IRouteAPIRabbitRPCService routeAPIRabbitProducer)
        {
            _logger = logger;
            _routeAPIRabbitRPCService = routeAPIRabbitProducer;
        }

        [Authorize]
        [HttpGet(Routes.RouteRoutes.GetAll)]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoutesRequest getAllRouteRequest)
        {
            if (!User.Claims.Any(x => x.Type == UserIdClaim)) return Unauthorized();
            var id = User.Claims.First(x => x.Type == UserIdClaim).Value;

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAllAsync)}: Recieved request.");

            var getAllRoutesRabbitRequest = new GetAllRoutesRabbitRequest()
            {
                UserId = id,
                FriendId = getAllRouteRequest.FriendId,
                Popular = getAllRouteRequest.Popular
            };

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAllAsync)}: Sending request to RouteService for method {GetAllRoutesMethod}.");
            var getAllRoutesRabbitResponse = await _routeAPIRabbitRPCService.PublishRabbitMessageWaitForResponseAsync<GetAllRoutesRabbitResponse>(GetAllRoutesMethod, getAllRoutesRabbitRequest);

            if (getAllRoutesRabbitResponse.FoundRoutes)
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAllAsync)}: Found routes.");
                return Ok(getAllRoutesRabbitResponse.Routes);
            }
            else
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAllAsync)}: No routes found.");
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet(Routes.RouteRoutes.Get)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            if (!User.Claims.Any(x => x.Type == UserIdClaim)) return Unauthorized();
            var userId = User.Claims.First(x => x.Type == UserIdClaim).Value;

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAsync)}: Recieved request.");

            var getRouteRabbitRequest = new GetRouteRabbitRequest()
            {
                Id = id,
                UserId = userId
            };

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(GetAsync)}: Sending request to RouteService for method {GetRouteMethod}.");
            var getRouteRabbitResponse = await _routeAPIRabbitRPCService.PublishRabbitMessageWaitForResponseAsync<GetRouteRabbitResponse>(GetRouteMethod, getRouteRabbitRequest);

            if (getRouteRabbitResponse.FoundRoute)
            {
                return Ok(getRouteRabbitResponse.Route);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost(Routes.RouteRoutes.Create)]
        public async Task<IActionResult> CreateAsync(CreateRouteRequest createRouteRequest)
        {
            if (!User.Claims.Any(x => x.Type == UserIdClaim)) return Unauthorized();
            var id = User.Claims.First(x => x.Type == UserIdClaim).Value;

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(CreateAsync)}: Recieved request.");

            var createRouteRabbitRequest = new CreateRouteRabbitRequest()
            {
                UserId = id,
                RouteName = createRouteRequest.RouteName,
                Places = createRouteRequest.Places
            };

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(CreateAsync)}: Sending request to RouteService for method {CreateRouteMethod}.");
            var createRouteRabbitResponse = await _routeAPIRabbitRPCService.PublishRabbitMessageWaitForResponseAsync<CreateRouteRabbitResponse>(CreateRouteMethod, createRouteRabbitRequest);

            if (createRouteRabbitResponse.Successful)
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(CreateAsync)}: Route created.");
                return Ok();
            }
            else
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(CreateAsync)}: Route creation failed.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete(Routes.RouteRoutes.Delete)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if (!User.Claims.Any(x => x.Type == UserIdClaim)) return Unauthorized();
            var userId = User.Claims.First(x => x.Type == UserIdClaim).Value;

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(DeleteAsync)}: Recieved request.");

            var deleteRouteRabbitRequest = new DeleteRouteRabbitRequest()
            {
                Id = id,
                UserId = userId
            };

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(DeleteAsync)}: Sending request to RouteService for method {DeleteRouteMethod}.");
            var deleteRouteRabbitResponse = await _routeAPIRabbitRPCService.PublishRabbitMessageWaitForResponseAsync<DeleteRouteRabbitResponse>(DeleteRouteMethod, deleteRouteRabbitRequest);

            if (deleteRouteRabbitResponse.Successful)
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(DeleteAsync)}: Route successfully deleted.");
                return Ok();
            }
            else
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(DeleteAsync)}: Route delete failed.");
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPut(Routes.RouteRoutes.Update)]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateRouteRequest updateRouteRequest)
        {
            if (!User.Claims.Any(x => x.Type == UserIdClaim)) return Unauthorized();
            var userId = User.Claims.First(x => x.Type == UserIdClaim).Value;

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(UpdateAsync)}: Recieved request.");

            if (updateRouteRequest.Like.HasValue && updateRouteRequest.Unlike.HasValue) return BadRequest();

            var updateRouteRabbitRequest = new UpdateRouteRabbitRequest()
            {
                Id = id,
                UserId = userId,
                Like = updateRouteRequest.Like,
                Unlike = updateRouteRequest.Unlike
            };

            _logger.LogInformation($"{nameof(RouteController)}.{nameof(UpdateAsync)}: Sending request to RouteService for method {UpdateRouteMethod}.");
            var updateRouteRabbitResponse = await _routeAPIRabbitRPCService.PublishRabbitMessageWaitForResponseAsync<UpdateRouteRabbitResponse>(UpdateRouteMethod, updateRouteRabbitRequest);

            if (updateRouteRabbitResponse.Successful)
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(UpdateAsync)}: Route {id} successfully updated.");
                return Ok();
            }
            else
            {
                _logger.LogInformation($"{nameof(RouteController)}.{nameof(UpdateAsync)}: Route {id} update failed.");
                return BadRequest();
            }
        }
    }
}
