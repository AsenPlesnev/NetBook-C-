namespace NetBook.Data.Models
{
    using System;

    using NetBook.Data.Common.Models;

    public class Subject : BaseDeletableModel<string>
    {
        public Subject()
        {
            this.CreatedOn = DateTime.UtcNow;
        }

        public string Name { get; set; }
    }
}
