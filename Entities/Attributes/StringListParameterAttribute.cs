using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Feipder.Entities.Attributes
{
    public class StringListParameterAttribute : ValidationAttribute
    {

        public StringListParameterAttribute()
        {
        }

        public override bool IsValid(object? value)
        {
            var regex = new Regex("^[0-9,]*$");

            if(value == null || regex.IsMatch(value.ToString()))
            {
                return true;
            }
         
            return false;

        }
    }
}
