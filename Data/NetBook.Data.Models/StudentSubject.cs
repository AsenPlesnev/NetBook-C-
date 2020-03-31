namespace NetBook.Data.Models
{
    public class StudentSubject
    {
        public string StudentId { get; set; }
        public Student Student { get; set; }

        public string SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}
