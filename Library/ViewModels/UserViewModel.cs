using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class UserViewModel
    {
        public virtual string Role { get; set; }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public virtual string SecondName { get; set; }

        public virtual string City { get; set; }
       
        public virtual string Street { get; set; }
       
        public virtual int Hous { get; set; }
        
        public virtual int Phone { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}
