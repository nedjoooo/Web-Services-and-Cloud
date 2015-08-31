namespace BookShopSystem.ConsoleClient
{
    using BookShopSystem.Data;
    using BookShopSystem.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class BookShopMain
    {
        static void Main()
        {
            // var context = new BookShopContext();

            //  1.	Get all books after the year 2000. Select only their titles.

            /*
            var booksAfterTheYear2000 = context.Books
                .Where(b => b.RelesaeDate.Year == 2000)
                .Select(b => b.Title)
                .ToList();

            foreach (var book in booksAfterTheYear2000)
            {
                Console.WriteLine("Book title: " + book);
            }
            */

            //  2.	Get all authors with at least one book with release date before 1990. Select their first name and last name.

            /*
            var authorsWithAtLeastOneBookWithReleaseDateBefore1990 = context.Authors
                .Where(a => a.Books.Where(b => b.RelesaeDate.Year < 1990).Count() > 0)
                .Select(a => new { a.FirstName, a.LastName})
                .ToList();

            foreach (var author in authorsWithAtLeastOneBookWithReleaseDateBefore1990)
            {
                Console.WriteLine(author.FirstName + " " + author.LastName);
            }
            */

            //  3.	Get all authors, ordered by the number of their books (descending). 
            //  Select their first name, last name and book count.

            /*
            var authorsOrderedByTheNumberOfTheirBooks = context.Authors
                .Select(a => new { a.Books.Count, a.FirstName, a.LastName })
                .OrderByDescending(a => a.Count)
                .ToList();

            foreach (var author in authorsOrderedByTheNumberOfTheirBooks)
            {
                Console.WriteLine("Books count: " + author.Count + ", Author name: " + author.FirstName + " " + author.LastName);
            }
            */

            //  4.	Get all books from author George Powell, ordered by their release date (descending), then by book title (ascending). 
            //  Select the book's title, release date and copies.

            /*
            var getBooksGeorgePowell = context.Books
                .Where(b => (b.Author.FirstName + " " + b.Author.LastName) == "George Powell")
                .OrderByDescending(b => b.RelesaeDate)
                .ThenBy(b => b.Title)
                .Select(b => new { b.Title, b.RelesaeDate, b.Copies })
                .ToList();

            foreach (var book in getBooksGeorgePowell)
            {
                Console.WriteLine(
                    "Title: " + book.Title + 
                    ", Release Date: " + book.RelesaeDate.ToString() + 
                    ", Copies: " + book.Copies);
            }
            */
             
            
            //  5.	Get the most recent books by categories. The categories should be ordered by total book count. 
            //   Only take the top 3 most recent books from each category - ordered by date (descending), then by title (ascending). 
            // Select the category name, total book count and for each book - its title and release date.

            /*
            var mostRecentBooksByCategory = context.Categories
                .OrderByDescending(c => c.Books.Count)
                .Select(c => new { c.Name, c.Books })
                .ToList();

            foreach (var category in mostRecentBooksByCategory)
            {
                var topBooks = category.Books.OrderByDescending(b => b.RelesaeDate).Take(3);
                Console.WriteLine("--" + category.Name + ": " + category.Books.Count);
                foreach (var book in topBooks)
                {
                    Console.WriteLine(book.Title + " (" + book.RelesaeDate.Year + ")");
                }
            }
            */

            //var books = context.Books
            //    .Take(3)
            //    .ToList();

            //books[0].RelatedBooks.Add(books[1]);
            //books[1].RelatedBooks.Add(books[0]);
            //books[0].RelatedBooks.Add(books[2]);
            //books[2].RelatedBooks.Add(books[0]);

            //context.SaveChanges();

            //var booksFromQuery = context.Books
            //    .Take(3)
            //    .ToList();

            //foreach (var book in booksFromQuery)
            //{
            //    Console.WriteLine("--{0}", book.Title);
            //    foreach (var relatedBook in book.RelatedBooks)
            //    {
            //        Console.WriteLine(relatedBook.Title);
            //    }
            //}


        }
    }
}
