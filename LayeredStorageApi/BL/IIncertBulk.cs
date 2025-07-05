using Project.Common.Models;

namespace LayeredStorageApi.BL
{
    public interface IIncertBulk
    {
        public Task<ResponseModel<int>> IncertBulkFromBody(string data);

    }
}
