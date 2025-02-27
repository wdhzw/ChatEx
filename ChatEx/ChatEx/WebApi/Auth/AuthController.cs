using Microsoft.AspNetCore.Mvc;

namespace ChatEx.WebApi.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpGet("GetToken")]
        public async Task GetToken()
        {
            await Task.Delay(50);
        }

        [HttpPost("Login")]
        public async Task Login()
        {
            await Task.Delay(50);
        }

        [HttpPost("Logout")]
        public async Task Logout()
        {
            await Task.Delay(50);
        }
    }
}
