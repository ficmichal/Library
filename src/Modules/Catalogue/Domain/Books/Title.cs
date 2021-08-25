using Library.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;

namespace Library.Modules.Catalogue.Domain.Books
{
    public class Title : ValueObject
    {
        public Title(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title cannot be empty!");
            }
            Value = title;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
