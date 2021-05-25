using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data.Models;
using Snackis.Services;

namespace Snackis.Pages.Admin
{
    public class ReportedMessagesModel : PageModel
    {
        private readonly MessageServices _messageServices;

        public ReportedMessagesModel(MessageServices messageServices)
        {
            _messageServices = messageServices;
        }

        public List<Message> ReportedMessages { get; set; }

        public async Task OnGetAsync()
        {
            ReportedMessages = await _messageServices.GetMessagesWithReportingsAsync();
        }
    }
}
