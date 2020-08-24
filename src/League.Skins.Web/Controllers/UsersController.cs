using System.Threading.Tasks;
using League.Skins.Core.Services;
using League.Skins.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace League.Skins.Web.Controllers
{
    public class UsersController : ApiController
    {
        private readonly UserService _userService;

        public UsersController(IHostEnvironment hostEnvironment, UserService userService) : base(hostEnvironment)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await _userService.Login(request);
            return ResolveResult(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await _userService.Register(request);
            return ResolveResult(result);
        }

        [HttpPost("relate")]
        public async Task<ActionResult> Relate([FromBody] UserRelateRequest request)
        {
            var result = await _userService.Relate("5f2f55f68023df09c4378184", request);
            return ResolveResult(result);
        }
    }
}
