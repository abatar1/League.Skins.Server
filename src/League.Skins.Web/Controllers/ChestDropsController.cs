using System.Threading.Tasks;
using League.Skins.Core;
using League.Skins.Core.Services;
using League.Skins.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace League.Skins.Web.Controllers
{
    public class ChestDropsController : ApiController
    {
        private readonly ChestDropService _chestDropService;

        public ChestDropsController(ChestDropService chestDropService, IHostEnvironment hostEnvironment) : base(hostEnvironment)
        {
            _chestDropService = chestDropService;
        }

        [HttpPost("add")]
        public async Task<ActionResult> Add([FromBody] ChestDropAddRequest request)
        {
            var chestDrop = await _chestDropService.Add("", request);
            return ResolveResult(chestDrop);
        }
    }
}
