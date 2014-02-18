using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Account
{
    public class UserProfilePoco
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Zа-яА-Я\\s]+$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*\\.([a-z]{2,4})$")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [RegularExpression("^[a-zA-Zа-яА-Я\\s]+$")]
        public string Country { get; set; }

        [RegularExpression("^[a-zA-Zа-яА-Я\\s]+$")]
        public string City { get; set; }

        [Required]
        [RegularExpression("^[^\f\n\r\t]{4,32}$")]
        public string Password { get; set; }

        public DateTime RegisterDate { get; set; }

        public int From { get; set; }
    }
}