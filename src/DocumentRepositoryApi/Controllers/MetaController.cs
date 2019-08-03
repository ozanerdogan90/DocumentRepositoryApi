using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentRepositoryApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class MetaController : ControllerBase
    {
        /// <summary>
        /// Readiness service to for k8s
        /// </summary>
        /// <returns></returns>
        [HttpGet("readiness")]
        public async Task<IActionResult> Readiness()
        {
            return Ok("I AM READY");
        }

        /// <summary>
        /// liveness service for k8s
        /// </summary>
        /// <returns></returns>
        [HttpGet("liveness")]
        public async Task<IActionResult> Liveness()
        {
            return Ok("I AM ALIVE");
        }
    }
}