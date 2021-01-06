using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using VHacksWebstore.Core.Domain;
using VHacksWebstore.Data;

namespace VHacksWebstore.Core.App.Pages.Webstore.Products
{
    public class CreateModel : PageModel
    {
        private readonly WebstoreDbContext _context;
        private readonly IMemoryCache _cache;

        public CreateModel(WebstoreDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        [BindProperty, Required]
        public Product Input { get; set; }
        [BindProperty, Required]
        public List<IFormFile> Images { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //List of file paths
            List<string> imageStreams = new();
            foreach (var file in Images)
            {
                new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out var contentType);
                if (!contentType.StartsWith("image/"))
                {
                    ModelState.AddModelError(string.Empty, "File type must be an image");
                    return Page();
                }

                switch (file.Length)
                {
                    case >= 100000000:
                        ModelState.AddModelError(string.Empty,"File is larger than 100mb"); 
                        continue;
                    case <= 0:
                        continue;
                }

                var inputStream = file.OpenReadStream();
                byte[] stream = new byte[inputStream.Length];
                Image image;
                string imagePath = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = inputStream.Read(stream, 0, stream.Length)) > 0)
                    {
                        ms.Write(stream, 0, read);
                    }
                    image = Image.FromStream(ms);
                    var imageId = Guid.NewGuid();
                    imagePath = $"/img/products/{imageId}.jpeg";
                    image.Save(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\img\products", $"{imageId}.jpeg"), ImageFormat.Jpeg);
                }
                imageStreams.Add(imagePath);
            }

            var product = new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = Input.Name,
                Description = Input.Description,
                Price = Input.Price,
                Images = imageStreams
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            _cache.Set("products", _context.Products.ToList());
            return RedirectToPage("./Index");
        }
    }
}
