using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class Author : ValueObject
    {
        public Author(string author)
        {
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentException("Author cannot be empty!");
            }
            Value = author;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
