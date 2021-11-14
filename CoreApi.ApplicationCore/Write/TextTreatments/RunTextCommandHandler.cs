using CoreApi.ApplicationCore.Contracts;
using CoreApi.ApplicationCore.Dtos;
using CoreApi.ApplicationCore.Read.Contracts;
using CoreApi.ApplicationCore.Write.Contracts;
using MediatR;

namespace CoreApi.ApplicationCore.Write.TextTreatments
{
    public record RunTextCommand(string Content) : IRequest;

    public class RunTextCommandHandler : RequestHandler<RunTextCommand>
    {
        private readonly IDispatcher<RunTextTreatmentDto> _dispatcher;
        private readonly ITimeService _timeService;
        private readonly ICurrentUserService _currentUserService;

        public RunTextCommandHandler(IDispatcher<RunTextTreatmentDto> dispatcher, ITimeService timeService,
            ICurrentUserService currentUserService)
        {
            _dispatcher = dispatcher;
            _timeService = timeService;
            _currentUserService = currentUserService;
        }

        protected override void Handle(RunTextCommand request)
        {
            
            _dispatcher.Dispatch(new RunTextTreatmentDto(_currentUserService.UserId.Value, request.Content,
                _timeService.Now()));
        }
    }
}