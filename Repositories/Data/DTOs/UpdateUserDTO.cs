using Repositories.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Data.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        [Required]
        public string Status { get; set; }
    }

}
