using Library.BuildingBlocks.Domain;
using System.Collections.Generic;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class Book : ValueObject
    {
        public Book(string isbn, string title, string author)
        {
            ISBN = new ISBN(isbn);
            Title = new Title(title);
            Author = new Author(author);
        }

        public ISBN ISBN { get; }

        public Title Title { get; }

        public Author Author { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ISBN;
        }
    }
}
