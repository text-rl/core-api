﻿using CoreApi.ApplicationCore.Contracts;
using CoreApi.Domain.Users;
using CoreApi.Client.Sse;
using Microsoft.AspNetCore.SignalR;

namespace CoreApi.Client.Services
{
    public class TextTreatmentMessageService : ITextTreatmentMessageService
    {
        private readonly ISseService _sseService;

        public TextTreatmentMessageService(ISseService sseService)
        {
            _sseService = sseService;
        }

        public void NotififyUser(UserId userId, TextTreatmentDoneEvent treatmentDoneEvent)
        {
            _sseService.SendToClientAsync(treatmentDoneEvent, userId.Value, "onTreatmentDone");
        }
    }
}