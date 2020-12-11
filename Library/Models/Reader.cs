using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Reader
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public int Phone { get; set; }
        public User User { get; set; }

    }
}
