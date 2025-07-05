using LayeredStorageApi.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;
using System.Text.Json;

namespace LayeredStorageApi.Controllers
{
    public class DataController : BaseApiController
    {
        private readonly IIncertBulk _incertBulk;

        public DataController(IIncertBulk incertBulk)
        {
            _incertBulk = incertBulk;
        }

        [HttpPost("incert-data")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<int>>> IncertData([FromBody] JsonElement data)
        {
            var res = await _incertBulk.IncertBulkFromBody(data.GetRawText());
            return StatusCode(res.StatusCode,res);
        }

        [HttpGet("get-daya-by-id/{id}")]
        [Authorize]
    }
}
