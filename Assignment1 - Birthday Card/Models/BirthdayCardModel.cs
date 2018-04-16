using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BirthdayCardGenerator.Models
{
    public class BirthdayCardModel
    {
        [Required(ErrorMessage = "Please enter the name of the recipient")]
        public string To { get; set; }
        [Required(ErrorMessage = "Please enter the name of the sender")]
        public string From { get; set; }
        [Required(ErrorMessage = "Please enter a message")]
        public string Message { get; set; }
    }
}