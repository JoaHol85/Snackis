using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data.Models;
using Snackis.Services;

namespace Snackis.Pages
{
    public class ReportMessagePageModel : PageModel
    {
        private readonly IMessageServices _messageServices;

        public ReportMessagePageModel(IMessageServices messageServices)
        {
            _messageServices = messageServices;
        }


        [BindProperty(SupportsGet = true)]
        public int ReportedMessageId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int DeleteImageId { get; set; }
        //[BindProperty(SupportsGet = true)]
        //public int DeleteMessageId { get; set; }
        [BindProperty]
        public int DeleteMessageId { get; set; }



        [BindProperty]
        public string MessageToAdmin { get; set; }
        [BindProperty]
        public string NewMessage { get; set; }


        public Message ReportedMessage { get; set; }
        public List<ReportedMessage> ListOfReports { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            if (ReportedMessageId == 0 && DeleteMessageId == 0)
            {
                return RedirectToPage("/Index");
            }
            if (DeleteImageId != 0)
            {
                await _messageServices.DeleteMessageImageAsync(DeleteImageId);
            }
            //OM INLOGGAD SOM ADMIN-->
            ReportedMessage = await _messageServices.GetSingleMessageAsync(ReportedMessageId);
            ListOfReports = await _messageServices.GetReportedMessagesAsync(ReportedMessageId);
            return null;
            //<--OM INLOGGAD SOM ADMIN
        }

        public async Task<IActionResult> OnPostChangeMessageAsync()
        {
            if (DeleteMessageId != 0)
            {
                await DeleteReportedMessageAndOtherAsync();
            }
            return Redirect("/admin/ReportedMessages");
        }


        public async Task OnPost()
        {
            ReportedMessage = await _messageServices.GetSingleMessageAsync(ReportedMessageId);
            ReportedMessage.TimesReported++;
            await _messageServices.ReportMessageAsync(ReportedMessage, MessageToAdmin);
        }

        public async Task DeleteReportedMessageAndOtherAsync()
        {
            ReportedMessage = await _messageServices.GetSingleMessageAsync(ReportedMessageId);
            await _messageServices.DeleterReportedMessagesAsync(DeleteMessageId);   //Ta bort alla anmälningar - klar
            await _messageServices.ChangeRemovedMessage(ReportedMessage, NewMessage);           //Ta bort alla "reports" - klar
        }
    }
}
