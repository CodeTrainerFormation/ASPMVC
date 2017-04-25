using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DomainModel.Validators
{
    public class PhoneNumber : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            String phone = Convert.ToString(value);

            Regex regPhoneNumber = new Regex(@"^(\+33 |0)[1-9]( \d\d){4}$");
            if (!regPhoneNumber.IsMatch(phone))
            {
                return new ValidationResult("Format invalide : +33 X XX XX XX XX ou XX XX XX XX XX sont autorisés");
            }

            return ValidationResult.Success;
        }

    }
}
