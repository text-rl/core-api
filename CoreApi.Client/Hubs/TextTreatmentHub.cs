using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CoreApi.Web.Hubs
{
    public interface ITextTreatmentClient
    {
        Task OnTextTreatmentDone(TextTreatmentDoneEvent? serverEvent);
    }

    public class TextTreatmentHub : Hub<ITextTreatmentClient>
    {
        public static string Route = "/texttreatmenthub";

       
    }
}