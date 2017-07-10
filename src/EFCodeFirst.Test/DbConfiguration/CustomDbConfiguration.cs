using System.Data.Entity;
using EFCodeFirst.Interceptors;
using EFCodeFirst.Test.Context;

namespace EFCodeFirst.Test.DbConfiguration
{
    public class CustomDbConfiguration : System.Data.Entity.DbConfiguration
    {
        public CustomDbConfiguration()
        {
            AddInterceptor(new FullTextSearchInterceptor());
            SetDatabaseInitializer(new CreateDatabaseIfNotExists<MyAppDbContext>());
        }
    }
}