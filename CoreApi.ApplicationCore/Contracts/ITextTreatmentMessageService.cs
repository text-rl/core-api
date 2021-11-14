using CoreApi.Domain.Users;

namespace CoreApi.ApplicationCore.Contracts
{
    public record TextTreatmentDoneEvent(string Result);

    public interface ITextTreatmentMessageService
    {
        public void NotififyUser(UserId userId, TextTreatmentDoneEvent treatmentDoneEvent);
    }
}