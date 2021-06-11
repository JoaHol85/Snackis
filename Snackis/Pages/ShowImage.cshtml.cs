using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Data;
using Snackis.Data.Models;

namespace Snackis.Pages
{
    public class ShowImageModel : PageModel
    {
        private readonly SnackisContext _context;

        public ShowImageModel(SnackisContext context)
        {
            _context = context;
        }


        [BindProperty(SupportsGet = true)]
        public int ImageId { get; set; }

        public string ImageURL { get; set; }


        public async Task OnGet()
        {
            MessageImage image = await _context.MessageImages.FindAsync(ImageId);

            string ImageUrl;
            MessageImage img = image;
            if (img == null)
            {
                ImageUrl = "http://placehold.it/300x300";
            }
            else
            {
                string imageBase64Data = Convert.ToBase64String(img.Data);
                ImageUrl = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            ImageURL = ImageUrl;
        }
    }
}
