using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snackis.Data;
using Snackis.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ThreadMessageController : ControllerBase
    {
        private readonly SnackisContext _context;

        public ThreadMessageController(SnackisContext context)
        {
            _context = context;
        }



        [HttpGet("{id}")]
        public SubThread GetMessagesInSubThreadById(int id)
        {
            var thread = _context.SubThreads.FirstOrDefault(s => s.Id == id);

            return thread;
        }
    }
}
