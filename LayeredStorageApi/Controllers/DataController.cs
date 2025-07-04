using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;

namespace LayeredStorageApi.Controllers
{
    public class DataController : BaseApiController
    {
        [HttpPost("incert-data")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<string>>> IncertData(string data)
        {
            return Ok(data);
        }
    }
}
