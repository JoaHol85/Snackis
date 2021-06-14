using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly SnackisContext _context;
        public MessageController(SnackisContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Message>>> GetMessagesFromSubThread(int id)
        {
            var list = await _context.Messages.Where(m => m.SubThreadId == id).ToListAsync();
            foreach (var message in list)
            {
                message.SnackisUser = new();
                message.SnackisUser.NickName = _context.Users.FirstOrDefault(u => u.Id == message.SnackisUserId).NickName;
                List<MessageImage> messageImages = new();
                messageImages = await _context.MessageImages.Where(m => m.MessageId == message.Id).ToListAsync();
                message.MessageImages = messageImages;
            }
            return list;
        }
    }
}
