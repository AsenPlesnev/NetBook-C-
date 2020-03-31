using System;
using System.Collections.Generic;
using System.Text;
using NetBook.Services.Mapping;

namespace NetBook.Services.Data.ServiceModels
{
    using NetBook.Data.Models;

    public class StudentServiceModel : IMapTo<Student>, IMapFrom<Student>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string PIN { get; set; }

        public string Citizenship { get; set; }

        public string DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Town { get; set; }

        public string Municipality { get; set; }

        public string Region { get; set; }

        public ClassServiceModel Class { get; set; }

        public string Address { get; set; }
    }
}
