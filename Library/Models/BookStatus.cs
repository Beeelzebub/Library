using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class BookStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookCopy> BookCopies { get; set; }
        public BookStatus()
        {
            BookCopies = new List<BookCopy>();
        }
    }
}
