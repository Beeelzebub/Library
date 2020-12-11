using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class UsageStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Usage> Usages { get; set; }
        public UsageStatus()
        {
            Usages = new List<Usage>();
        }
    }
}
