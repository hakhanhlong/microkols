using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Website.Code;
using Website.Code.Helpers;

namespace Website.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private readonly IFacebookHelper _facebookHelper;
        private readonly AppOptions _options;
        public FacebookController(IFacebookHelper facebookHelper, IOptionsMonitor<AppOptions> optionsAccessor)
        {
            _facebookHelper = facebookHelper;
            _options = optionsAccessor.CurrentValue;
        }

        public async Task<IActionResult> GetPosts(string fid = "10211187493683462")
        {
            var accessToken = "EAAJeZAPgrwW4BADZA3bjjkrpMBvKVAaL5tjosqIFHOfq76xSNOy8RlW1l8nwwZBWtF1pc06d0VHrC7r4ETepwJ32wX0uoMcdVrsZB82iVBBgSneDbNfHfXdwd1yyjo8JnIY0NPUYVKiEsRaeVHk6eeHZBX4odLlsZD";
            var model = await _facebookHelper.GetPosts(accessToken, fid);
            return Ok(model);
        }
    }
}