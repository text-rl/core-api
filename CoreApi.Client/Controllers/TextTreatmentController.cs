using System.Threading.Tasks;
using CoreApi.ApplicationCore;
using CoreApi.ApplicationCore.Write.TextTreatments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreApi.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextTreatmentController : ControllerBase
    {
        private readonly ILogger<TextTreatmentController> _logger;
        private readonly ISender _sender;

        public TextTreatmentController(ILogger<TextTreatmentController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Post(RunTextCommand cmd)
        {
            await _sender.Send(cmd);
            return Ok();
        }
    }
}