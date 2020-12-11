using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Имя*")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия*")]
        public string SecondName { get; set; }

        [Required]
        [Display(Name = "Город*")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Улица*")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Дом*")]
        public int Hous { get; set; }

        [Required]
        [Display(Name = "Телефон*")]
        public int Phone { get; set; }

        [Required]
        [Display(Name = "Логин*")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль*")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
