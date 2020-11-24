using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class SecondName
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Reader> Readers { get; set; }
        public List<Librarian> Librarians { get; set; }
        public SecondName()
        {
            Readers = new List<Reader>();
            Librarians = new List<Librarian>();
        }
    }
}
