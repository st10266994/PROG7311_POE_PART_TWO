using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PROG7311_PART_TWO.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
    }
}