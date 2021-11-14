using System;
using System.Threading;
using System.Threading.Tasks;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;
using MediatR;

namespace CoreApi.ApplicationCore.Write.TextTreatments
{
    public record TreatmentDoneNotification(UserId UserId, string Result) : INotification;

    public class TreatmentDoneNotificationHandler : INotificationHandler<TreatmentDoneNotification>
    {
        private ITextTreatmentMessageService _textTreatmentMessageService;


        public TreatmentDoneNotificationHandler(ITextTreatmentMessageService textTreatmentMessageService)
        {
            _textTreatmentMessageService = textTreatmentMessageService;
        }


        public Task Handle(TreatmentDoneNotification notification, CancellationToken cancellationToken)
        {
            _textTreatmentMessageService.NotififyUser(notification.UserId,
                new TextTreatmentDoneEvent(notification.Result));
            return Task.CompletedTask;
        }
    }
}