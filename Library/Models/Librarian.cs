using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Librarian
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public User User { get; set; }
    }
}
