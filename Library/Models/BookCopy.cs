using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public string Notes { get; set; }
        public BookStatus BookStatus { get; set; }
    }
}
