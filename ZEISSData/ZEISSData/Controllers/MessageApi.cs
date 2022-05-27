using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ZEISSData.Models;
using ZEISSData.Services;

namespace ZEISSData.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageApi : ControllerBase
    {
        private readonly IMessageBusinessObject messageBusinessObject;

        public MessageApi(IMessageBusinessObject messageBo) {
            this.messageBusinessObject = messageBo;
            if (!Program.dbInitilized) {
                messageBusinessObject.InitializeDbSchema();
                Program.dbInitilized = true;
            }
        }

        [HttpGet("list")]
        public IEnumerable<MessageModel> Get()
        {
            var message = this.messageBusinessObject.GetAllMessages();
            return message;
        }
    }
}
