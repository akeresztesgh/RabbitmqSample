using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Sender.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendController : ControllerBase
    {
        private readonly IRabbitMqSender rabbitMqSender;

        public SendController(IRabbitMqSender rabbitMqSender)
        {
            this.rabbitMqSender = rabbitMqSender;
        }

        [HttpPost, Route("")]
        public IActionResult SendMessage([FromBody] string message)
        {
            if(string.IsNullOrWhiteSpace(message))
            {
                return BadRequest();
            }
            rabbitMqSender.SendMessage(message);
            return Ok();
        }
    }
}
