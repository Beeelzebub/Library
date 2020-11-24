using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class FirstName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Reader> Readers { get; set; }
        public List<Librarian> Librarians { get; set; }
        public FirstName()
        {
            Readers = new List<Reader>();
            Librarians = new List<Librarian>();
        }
    }
}
