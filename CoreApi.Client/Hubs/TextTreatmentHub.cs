using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;
using Microsoft.AspNetCore.SignalR;

namespace CoreApi.Web.Hubs
{
    public interface ITextTreatmentClient
    {
        Task OnTextTreatmentDone(TextTreatmentDoneEvent? serverEvent);
    }

    public class TextTreatmentHub : Hub<ITextTreatmentClient>, ITextTreatmentMessageService
    {
        public static string Route = "/texttreatmenthub";

        public void NotififyUser(UserId userId, TextTreatmentDoneEvent treatmentDoneEvent)
        {
            Clients.User(userId.Value.ToString()).OnTextTreatmentDone(treatmentDoneEvent);
        }
    }
}