using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModel.Response
{
    public class AccountResponse
    {
        public int CustomerId { get; set; }

        public string? CustomerFullName { get; set; }

        public string? Telephone { get; set; }

        public string EmailAddress { get; set; } = null!;


    }
}
