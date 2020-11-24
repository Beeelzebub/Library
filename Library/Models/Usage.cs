using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Usage
    {
        public int Id { get; set; }
        public BookCopy BookCopy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Librarian Librarian { get; set; }
        public Reader Reader { get; set; }
        public UsageStatus UsageStatus { get; set; }
    }
}
