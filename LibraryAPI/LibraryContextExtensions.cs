using LibraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI
{
    public static class LibraryContextExtensions
    {
        public static void EnsureSeedDataForContext(this LibraryContext context)
        {
            if (context.Authors.Any())
            {
                return;
            }

            //init seed data
            Publisher publisher = new Publisher()
            {
                Name = "Znak"
            };
            var authors = new List<Author>()
            {
                new Author()
                {
                    FirstName = "Donna",
                    LastName = "Tartt",
                    Books = new List<Book>()
                    {
                        new Book()
                        {
                            Title = "Szczygieł",
                            ISBN = 9788324026524,
                            Publisher = publisher
                        },
                        new Book()
                        {
                            Title = "Tajemna Historia",
                            ISBN = 9788324027415,
                            Publisher = publisher
                        },
                        new Book()
                        {
                            Title = "Mały Przyjaciel",
                            ISBN = 9788324035571,
                            Publisher = publisher

                        }
                    }
                },
                new Author()
                {
                    FirstName = "James",
                    LastName = "Jones",
                    Books = new List<Book>()
                    {
                        new Book()
                        {
                            Title = "From Here To Eternity",
                            ISBN = 8497898745,
                            Publisher = new Publisher()
                            {
                                Name = "Książnica"                             
                            }
                        }
                    }
                }
            };
            context.Authors.AddRange(authors);
            context.SaveChanges();
        }
    }
}
