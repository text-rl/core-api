using System;
using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Dtos;
using MediatR;

namespace CoreApi.ApplicationCore
{
    public record RunTextCommand(string Content) : IRequest;

    public class RunTextCommandHandler : RequestHandler<RunTextCommand>
    {
        private readonly IDispatcher<RunTextTreatmentDto> _dispatcher;
        private readonly ITimeService _timeService;

        public RunTextCommandHandler(IDispatcher<RunTextTreatmentDto> dispatcher, ITimeService timeService)
        {
            _dispatcher = dispatcher;
            _timeService = timeService;
        }

        protected override void Handle(RunTextCommand request)
        {
            _dispatcher.Dispatch(new RunTextTreatmentDto(Guid.NewGuid(), request.Content, _timeService.Now()));
        }
    }
}