using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantApplication.Api.Managers;
using RestaurantApplication.Api.Models;

namespace RestaurantApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        private readonly CoreManager coreManager;
        public CoreController(CoreManager coreManager)
        {
            this.coreManager = coreManager ?? throw new ArgumentNullException(nameof(coreManager));
        }

        [HttpGet("tables")]
        public async Task<ActionResult<IEnumerable<Table>>> GetTables()
        {
            return Ok(await coreManager.GetTables());
        }
    }
}