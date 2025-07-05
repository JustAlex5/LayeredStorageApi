using Microsoft.AspNetCore.Mvc;
using Project.Common.Models;

namespace LayeredStorageApi.Services.Interfaces
{
    public interface IIncertBulk
    {
        public Task<ResponseModel<int>> IncertBulkFromBody(string data);
        public Task<ResponseModel<string>> GetDataByIdAsync(int id);
        public Task<ResponseModel<bool>> UpdateDataByIdAsync(int id, string data);




    }
}
