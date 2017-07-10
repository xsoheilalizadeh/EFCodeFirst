using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using EFCodeFirst.Test.DomainClass;

namespace EFCodeFirst.Test.Maps
{
    /// <summary>
    /// Here use Fluent API For Mapping Entities
    /// </summary>
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            HasKey(person => person.Id); // This Is Optional, When Your Key Name Is "Id" and EndedWith "Id" Like "PersonId"
            Property(person => person.Description)
                .HasMaxLength(200)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Description")));
        }
    }
}