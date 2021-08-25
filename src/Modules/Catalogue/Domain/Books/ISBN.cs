using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class ISBN : ValueObject
    {
        private const string VerySimpleISBNCheck = "^\\d{9}[\\d|X]$";

        public string Value { get; }

        public ISBN(string isbn)
        {
            if (!Regex.IsMatch(isbn.Trim(), VerySimpleISBNCheck))
            {
                throw new ArgumentException("Wrong ISBN!");
            }
            Value = isbn;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
