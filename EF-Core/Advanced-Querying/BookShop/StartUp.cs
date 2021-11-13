using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using BookShop.Models.Enums;
using Z.BulkOperations;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);
            Console.WriteLine(RemoveBooks(db));
            
        }
        //2. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context
                .Books
                .ToArray()
                .Where(b => b.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(b => b.Title)
                .OrderBy(t=>t)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var b in books)
            {
                sb.AppendLine(b);
            }

            return sb.ToString().TrimEnd();
        }
        //3. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context
                .Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var goldenBook in goldenBooks)
            {
                sb.AppendLine(goldenBook.Title);
            }

            return sb.ToString().TrimEnd();
        }
        //4. Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context
                .Books
                .ToArray()
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    Price = b.Price.ToString("f2")
                })
                .OrderByDescending(b=>b.Price)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }
        //5. Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var titles = context
                .Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
        //6. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c=>c.ToLower())
                .ToArray();

            var titles = context
                .Books
                .Where(b=>b.BookCategories.Any(bc=>categories.Contains(bc.Category.Name.ToLower())))
                .Select(b => new 
                {
                    Title=b.Title
                })
                .OrderBy(t => t.Title)
                .ToArray();
            StringBuilder sb = new StringBuilder();

            foreach (var title in titles)
            {
                sb.AppendLine(title.Title);
            }

            return sb.ToString().TrimEnd();

        }
        //7. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var beforeDateTime = DateTime.ParseExact(date, "dd-MM-yyyy",CultureInfo.InvariantCulture);

            var releaseBefore = context
                .Books
                .Where(b => b.ReleaseDate < beforeDateTime)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    Title = b.Title,
                    Type = b.EditionType,
                    Price = b.Price
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in releaseBefore)
            {
                sb.AppendLine($"{book.Title} - {book.Type} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();


        }
        //8. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .ToArray()
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    FullName = $"{a.FirstName} {a.LastName}"
                })
                .OrderBy(a => a.FullName)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }
        //9. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {

            var titles = context
                .Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }

            return sb.ToString().TrimEnd();
        }
        //10. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var titles = context
                .Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b=>b.BookId)
                .Select(b => new
                {
                    b.Title,
                    Author=$"{b.Author.FirstName} {b.Author.LastName}"
                })
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var title in titles)
            {
                sb.AppendLine($"{title.Title} ({title.Author})");
            }

            return sb.ToString().TrimEnd();
        }
        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksLength = context
                .Books
                .Where(b => b.Title.Length > lengthCheck)
                .ToArray();

            return booksLength.Length;
        }
        //12. Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorCopies = context
                .Authors
                .Select(b => new
                {
                    Author = $"{b.FirstName} {b.LastName}",
                    Copies = b.Books.Sum(x => x.Copies)
                })
                .OrderByDescending(ac=>ac.Copies)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var ac in authorCopies)
            {
                sb.AppendLine($"{ac.Author} - {ac.Copies}");
            }

            return sb.ToString().TrimEnd();
        }
        //13. Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfit = context
                .Categories
                .Select(c => new
                {
                    c.Name,
                    Profit = c.CategoryBooks.Sum(b => b.Book.Price * b.Book.Copies)
                })
                .OrderByDescending(c=>c.Profit)
                .ThenBy(c=>c.Name)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var category in categoryProfit)
            {
                sb.AppendLine($"{category.Name} ${category.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        //14. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                        .Select(b=>new
                        {
                            b.Book.Title,
                            b.Book.ReleaseDate
                        })
                        .OrderByDescending(b=>b.ReleaseDate)
                        .Take(3)
                        .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        //15. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context
                .Books
                .Where(b => b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
            context.BulkUpdate(books);

        }
        //16. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context
                .Books
                .Where(b => b.Copies < 4200);
            int count = booksToRemove.ToArray().Length;
            context.BulkDelete(booksToRemove);

            return count;
        }
    }
}
