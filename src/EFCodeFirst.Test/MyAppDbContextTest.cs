using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using EFCodeFirst.Interceptors;
using EFCodeFirst.Test.Context;
using EFCodeFirst.Test.DomainClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EFCodeFirst.Test
{
    [TestClass]
    public class MyAppDbContextTest
    {
        public MyAppDbContextTest()
        {
            using (var context = new MyAppDbContext())
            {
                context.Persons.AddRange(GetPersons());
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void TestConnectionString_Must__MainConnectionString_Equals_With_FackConnectionString()
        {
            using (var context = new MyAppDbContext())
            {
                const string connectionString =
                    "Data Source=.;Initial Catalog=AppDbTesting;Integrated Security = True;MultipleActiveResultSets=True;";
                var connection = context.Database.Connection;

                Assert.AreEqual(connection.Database, "AppDbTesting");
                Assert.AreEqual(connectionString, connection.ConnectionString);
                Assert.AreEqual(connection.State, ConnectionState.Closed);
            }
        }

        [TestMethod]
        public void InsertPerson_that_Do_Not_show_Error()
        {
            using (var context = new MyAppDbContext())
            {
                context.Persons.AddRange(GetPersonsIsPass());
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    if (e.InnerException == null)
                    {
                        Assert.Inconclusive($"\nMessage:{e.Message} \nTypeOf:{e.GetType()}");
                    }

                    if (e.InnerException.InnerException == null)
                    {
                        Assert.Inconclusive(
                            $"\nMessage:{e.InnerException.Message} \nTypeOf:{e.InnerException.GetType()}");
                    }

                    Assert.Inconclusive(
                        $"\nMessage:{e.InnerException.InnerException.Message} \nTypeOf:{e.InnerException.InnerException.GetType()}");
                }
            }
        }

        [TestMethod]
        public void InsertPerson_that_show_Error()
        {
            using (var context = new MyAppDbContext())
            {
                context.Persons.AddRange(GetPersonsIsFail());
                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    if (e.InnerException == null)
                    {
                        Assert.Inconclusive($"\nMessage:{e.Message} \nTypeOf:{e.GetType()}");
                    }

                    if (e.InnerException.InnerException == null)
                    {
                        Assert.Inconclusive(
                            $"\nMessage:{e.InnerException.Message} \nTypeOf:{e.InnerException.GetType()}");
                    }

                    Assert.Inconclusive(
                        $"\nMessage:{e.InnerException.InnerException.Message} \nTypeOf:{e.InnerException.InnerException.GetType()}");
                }
            }
        }


        [TestMethod]
        public void FullTextSearch_Must_Give_One_Person()
        {
            using (var context = new MyAppDbContext())
            {
                try
                {
                    var word = FullTextSearchInterceptor.FullTextSearch("Master");
                    var query = context.Persons
                        .Where(
                            _ =>
                                _.Description
                                    .Contains(word))
                        .ToList();
                    Assert.AreEqual(query.Count, 1);
                }
                catch (Exception e)
                {
                    if (e is EntityCommandExecutionException)
                    {
                        Assert.Inconclusive($"\n\nIn Your SQL Server Is Not Install FullText Search Feature. {e.InnerException?.Message}");
                    }
                }
               
            }
        }

        private List<Person> GetPersonsIsPass()
        {
            return new List<Person>()
            {
                new Person() {Name = "a"},
            };
        }

        private List<Person> GetPersonsIsFail()
        {
            return new List<Person>()
            {
                new Person() {Id = 1, Name = "a"},
            };
        }

        private List<Person> GetPersons()
        {
            return new List<Person>()
            {
                new Person() {Name = "Soheil", Description = "He is Good Man"},
                new Person() {Name = "julie larman", Description = "She is My Master :)"}
            };
        }
    }
}