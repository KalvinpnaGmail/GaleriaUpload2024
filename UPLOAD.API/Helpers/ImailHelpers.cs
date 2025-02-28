using UPLOAD.SHARE.Response;

namespace UPLOAD.API.Helpers
{
    public interface ImailHelpers
    {
        ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}