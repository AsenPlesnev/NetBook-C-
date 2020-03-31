namespace NetBook.Data.Seeding
{
    using System.Collections.Generic;
    using System.Linq;

    using NetBook.Data.Models;
    using NetBook.Data.Models.Enums;

    public class DataSeeder
    {
        private readonly ApplicationDbContext context;

        public DataSeeder(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void SeedSubjects()
        {
            if (!this.context.Subjects.Any())
            {
                this.context.Subjects.AddRange(new List<Subject>
                {
                    new Subject
                    {
                        Name = "Английски език",
                    },
                    new Subject
                    {
                        Name = "Математика",
                    },
                    new Subject
                    {
                        Name = "Информатика",
                    },
                });

                this.context.SaveChanges();
            }
        }

        public void SeedSchool()
        {
            if (!this.context.School.Any())
            {
                this.context.School.AddRange(new List<School>
                {
                    new School
                    {
                        Name = "ПМГ \"Акад. Сергей П. Корольов\"",
                        Town = "Благоевград",
                        Municipality = "Благоевград",
                        Region = "Благоевград",
                        Area = "Благоевград",
                    },
                });

                this.context.SaveChanges();
            }
        }
    }
}
