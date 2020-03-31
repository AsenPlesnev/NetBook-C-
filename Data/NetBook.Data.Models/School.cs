namespace NetBook.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NetBook.Data.Common.Models;

    public class School : BaseDeletableModel<string>
    {
        public School()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Town { get; set; }

        [Required]
        public string Municipality { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Area { get; set; }
    }
}
