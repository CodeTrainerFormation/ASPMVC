using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NetSchoolWeb.Models
{
    abstract public class Person
    {
        public int PersonID { get; set; }

        [Required(ErrorMessage = "Please enter a first name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }

        [Range(0, 130)]
        public int Age { get; set; }

        [Required]
        //[RegularExpression(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9._]+\.[a-zA-Z{2,4}$")]
        public string Email { get; set; }
    }
}