using System;
using System.Collections.Generic;
using System.Text;
using NetBook.Data.Models;
using NetBook.Services.Mapping;

namespace NetBook.Services.Data.ServiceModels
{
    public class ClassSubjectServiceModel : IMapFrom<SubjectClass>, IMapTo<SubjectClass>
    {
        public string Id { get; set; }

        public string SubjectId { get; set; }

        public SubjectServiceModel Subject { get; set; }

        public string ClassId { get; set; }

        public ClassServiceModel Class { get; set; }

        public int Workload { get; set; }

    }
}
