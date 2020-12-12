using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModels
{
    public class BookCopyViewModel
    {
        public int BookId { get; set; }
        public string Notes { get; set; }
        public IFormFile Image { get; set; }
    }
}
