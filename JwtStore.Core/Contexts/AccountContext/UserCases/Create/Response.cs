using Flunt.Notifications;

namespace JwtStore.Core.Contexts.AccountContext.UserCases.Create
{
    public class Response : SharedContext.UseCases.Response
    {
        protected Response() { }

        public Response(string message, int status, IEnumerable<Notification>? notification = null)
        {
            Message = message;
            Status = status;
            Notifications = notification;
        }

        public Response(string message, ResponseData data)
        {
            Message = message;
            Status = 200;
            Notifications = null;
            Data = data;
        }

        public ResponseData? Data { get; set; }
    }

    public record ResponseData(Guid Id, String Name, String Email);
}