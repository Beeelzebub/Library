using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class ReaderViewModel : UserViewModel
    {
        [Required]
        [Display(Name = "Город")]
        public override string City { get; set; }

        [Required]
        [Display(Name = "Улица")]
        public override string Street { get; set; }

        [Required]
        [Display(Name = "Дом")]
        public override int Hous { get; set; }

        [Required]
        [Display(Name = "Телефон")]
        public override int Phone { get; set; }
    }
}
