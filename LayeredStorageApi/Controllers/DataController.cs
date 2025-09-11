using LayeredStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Dtos.Data;
using Project.Common.Models;
using Project.Common.Utils;
using System.Text.Json;

namespace LayeredStorageApi.Controllers
{
    public class DataController : BaseApiController
    {
        private readonly IInsertDal _insertDal;

        public DataController(IInsertDal insertDal)
        {
            _insertDal = insertDal;
        }

        [HttpPost("incert-data")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ResponseModel<int>>> IncertData([FromBody] JsonElement data)
        {
            var res = await _insertDal.InsertBulkFromBody(data.GetRawText());
            return StatusCode(res.StatusCode,res);
        }

        [HttpGet("get-data-by-id/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<string>>> GetDataById(int id)
        {
            var res = await _insertDal.GetDataByIdAsync(id);
            return StatusCode(res.StatusCode,res);

        }

        [HttpPut("update-data/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseModel<bool>>> UpdateDataById(int id, [FromBody] UpdateDataDto dto)
        {
            var rawData = dto.Data.GetRawText();
            var res = await _insertDal.UpdateDataByIdAsync(id, rawData);
            return StatusCode(res.StatusCode, res);
        }




    }
}
