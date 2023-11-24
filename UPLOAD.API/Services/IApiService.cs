using UPLOAD.SHARE.Responses;

namespace UPLOAD.API.Services
{
    public interface IApiService
    {
        Task<Response> GetListASync<T>(string servicePrefix, string controller);
    }
}
