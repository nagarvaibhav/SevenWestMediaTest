using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SevenWestTest.Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                if (result.Any())
                {
                    return Ok(result);
                }
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Get User");
                return new StatusCodeResult(500);
            }
        }
    }
}
