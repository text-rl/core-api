using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;
using CoreApi.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CoreApi.Web.Services
{
    public class TextTreatmentMessageService : ITextTreatmentMessageService
    {
        private readonly IHubContext<TextTreatmentHub, ITextTreatmentClient> _hub;

        public TextTreatmentMessageService(IHubContext<TextTreatmentHub, ITextTreatmentClient> hub)
        {
            _hub = hub;
        }

        public void NotififyUser(UserId userId, TextTreatmentDoneEvent treatmentDoneEvent)
        {
            var id = userId.Value.ToString();
            _hub.Clients.User(id).OnTextTreatmentDone(treatmentDoneEvent);
        }
    }
}