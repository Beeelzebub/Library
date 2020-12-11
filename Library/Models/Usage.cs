using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Usage
    {
        public int Id { get; set; }
        public int BookCopyId { get; set; }
        public BookCopy BookCopy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartLibrarianId { get; set; }
        public Librarian StartLibrarian { get; set; }
        public string EndLibrarianId { get; set; }
        public Librarian EndLibrarian { get; set; }
        public string ReaderId { get; set; }
        public Reader Reader { get; set; }
        public int UsageStatusId { get; set; }
        public UsageStatus UsageStatus { get; set; }
    }
}
