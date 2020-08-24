using System;
using League.Skins.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace League.Skins.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ApiController : Controller
    {
        private readonly IHostEnvironment _hostEnvironment;

        protected ApiController(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        protected ActionResult ResolveResult(ServiceResponse serviceResponse)
        {
            if (!serviceResponse.HasError) return Ok(serviceResponse);

            if (_hostEnvironment.IsProduction())
            {
                serviceResponse.ErrorResponse.Message = null;
                serviceResponse.ErrorResponse.StackTrace = null;
            }

            switch (serviceResponse.ErrorResponse.Code)
            {
                case ErrorServiceCodes.WrongEnumFormat:
                case ErrorServiceCodes.ChestDropInvalidModel:
                    return UnprocessableEntity(serviceResponse);
                case ErrorServiceCodes.EntityAlreadyExists:
                    return Conflict(serviceResponse);
                case ErrorServiceCodes.WrongLoginOrPassword:
                    return Unauthorized(serviceResponse);
                case ErrorServiceCodes.UserNotRelated:
                    return StatusCode(StatusCodes.Status403Forbidden, serviceResponse);
                case ErrorServiceCodes.EntityNotFound:
                    return NotFound(serviceResponse);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
