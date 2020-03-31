namespace NetBook.Services.Data.Tests.Common
{
    using System;

    using Microsoft.EntityFrameworkCore;

    using NetBook.Data;

    public class NetBookDbContextInMemoryFactory
    {
        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
