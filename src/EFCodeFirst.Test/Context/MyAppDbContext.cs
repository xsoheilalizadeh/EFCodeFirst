using System.Data.Entity;
using EFCodeFirst.Test.DomainClass;
using EFCodeFirst.Test.Maps;

namespace EFCodeFirst.Test.Context
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext()
            :base("Data Source=.;Initial Catalog=AppDbTesting;Integrated Security = True;MultipleActiveResultSets=True;")
        {
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PersonMap());
        }
    }
}