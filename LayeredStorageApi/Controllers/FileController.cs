using LayeredStorageApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;
using Project.Common.Utils.Helpers;

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
            if (file == null || file.Length == 0)
                return BadRequest(ResponseFactory.Error<int>("No file uploaded"));

            using var stream = file.OpenReadStream();

            var res = await _fileService.UploadFileAsync(stream, file.FileName, file);

            if (res == null)
                return StatusCode(500, ResponseFactory.Error<int>("File upload failed"));

            return StatusCode(res.StatusCode,res);
        }

    }
}
