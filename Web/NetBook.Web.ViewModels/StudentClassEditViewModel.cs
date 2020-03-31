using System.ComponentModel.DataAnnotations;
using NetBook.Data.Models;

namespace NetBook.Web.ViewModels
{
    public class StudentClassEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string PIN { get; set; }

        [Required]
        public string Citizenship { get; set; }

        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
