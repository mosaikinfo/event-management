using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EventManagement.Infrastructure.Data
{
    public class EventsDbContextFactory : IDesignTimeDbContextFactory<EventsDbContext>
    {
        public EventsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventsDbContext>();
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EventManagement;Trusted_Connection=True;");
            return new EventsDbContext(optionsBuilder.Options);
        }
    }
}
