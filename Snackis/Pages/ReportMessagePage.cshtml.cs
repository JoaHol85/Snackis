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
        private readonly MessageServices _messageServices;

        public ReportMessagePageModel(MessageServices messageServices)
        {
            _messageServices = messageServices;
        }


        [BindProperty(SupportsGet = true)]
        public int ReportedMessageId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int DeleteMessageId { get; set; }



        [BindProperty]
        public string MessageToAdmin { get; set; }


        public Message ReportedMessage { get; set; }
        public List<ReportedMessage> ListOfReports { get; set; }


        public async Task OnGetAsync()
        {
            if (ReportedMessageId == 0 && DeleteMessageId == 0)
            {
                RedirectToPage("/Index");
            }
            //OM INLOGGAD SOM ADMIN-->
            ReportedMessage = await _messageServices.GetSingleMessage(ReportedMessageId);
            ListOfReports = await _messageServices.GetReportedMessagesAsync(ReportedMessageId);

            if (DeleteMessageId != 0)
            {
                await DeleteReportedMessageAndOtherAsync();
            }
            //<--OM INLOGGAD SOM ADMIN
        }
        public async Task OnPost()
        {
            ReportedMessage = await _messageServices.GetSingleMessage(ReportedMessageId);
            ReportedMessage.TimesReported++;
            await _messageServices.ReportMessageAsync(ReportedMessage, MessageToAdmin);
        }

        public async Task DeleteReportedMessageAndOtherAsync()
        {
            await _messageServices.DeleterReportedMessagesAsync(DeleteMessageId);   //Ta bort alla anmälningar - klar
            await _messageServices.ChangeRemovedMessage(ReportedMessage);           //Ta bort alla "reports" - klar
            //Ändra meddelandet till typ "meddelande raderat" och ta bort möjligheten att anmäla inlägg.
            //KAN HA OLIKA FÖRUTBESTÄMDA MEDDELANDEN SOM SÄTTS IN ISTÄLLET FÖR ORGINALMEDDELANDET.

        }
    }
}
