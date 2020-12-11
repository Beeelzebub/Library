using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string Notes { get; set; }
        public bool IsInStock { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}
