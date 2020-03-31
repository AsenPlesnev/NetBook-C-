namespace NetBook.Web.ViewModels.Class
{
    using NetBook.Services.Mapping;
    using NetBook.Services.Models;

    public class AllStudentsInClassViewModel : IMapFrom<StudentServiceModel>
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string PIN { get; set; }

        public string PhoneNumber { get; set; }
    }
}
