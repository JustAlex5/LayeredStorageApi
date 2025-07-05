using Microsoft.AspNetCore.Mvc;
using Project.Common.Enums;
using Project.Common.Models;

namespace LayeredStorageApi.Services.Interfaces
{
    public interface IIncertBulk
    {
        Task<ResponseModel<int>> IncertBulkFromBody(string data, SourceTypeEnum source = SourceTypeEnum.Manual);
        Task<ResponseModel<string>> GetDataByIdAsync(int id);
        Task<ResponseModel<bool>> UpdateDataByIdAsync(int id, string data);




    }
}
