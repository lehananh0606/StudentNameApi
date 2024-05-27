using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel.Request
{
    public class AccountRequestCreate
    {
        [Required]
        public string? CustomerFullName { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "The phone number must be 10 characters long.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Invalid phone number format. 0[3|5|7|8|9] + 8 digits.")]
        public string? Telephone { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string EmailAddress { get; set; } = null!;

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CustomerBirthday { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
