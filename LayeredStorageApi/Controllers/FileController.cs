using LayeredStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;

namespace LayeredStorageApi.Controllers
{
    public class FileController :BaseApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        //[Authorize]
        public async Task<ActionResult<ResponseModel<int>>> UploadFile(IFormFile file)
        {
            return Ok();
        }

    }
}
