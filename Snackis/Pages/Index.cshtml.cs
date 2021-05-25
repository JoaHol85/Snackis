using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Snackis.Data.Models;
using Snackis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MainThreadServices _mainServices;

        public IndexModel(ILogger<IndexModel> logger, MainThreadServices mainServices)
        {
            _mainServices = mainServices;
            _logger = logger;
        }

        public List<MainThread> MainThreads { get; set; }


        public async Task OnGetAsync()
        {
            MainThreads = await _mainServices.GetMainThreadsAsync();
        }
    }
}
