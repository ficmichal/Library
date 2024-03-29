﻿using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using Library.Modules.Lending.Domain.LibraryBranch;
using Library.Modules.Lending.Domain.Patrons;
using System;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.UnitTests.Shared.Fixtures.Books
{
    public class BookFixture
    {
        public static Version Version0 => Version.Zero();

        public static BookId AnyBookId => new(Guid.NewGuid());

        public static LibraryBranchId AnyBranchId => new(Guid.NewGuid());

        public static PatronId AnyPatronId => new(Guid.NewGuid());

        public static AvailableBook CirculatingBook()
        {
            return new(AnyBookId, BookType.Circulating, Version0, AnyBranchId);
        }

        public static AvailableBook CirculatingAvailableBookAt(LibraryBranchId libraryBranchId)
        {
            return new(AnyBookId, BookType.Circulating, Version0, libraryBranchId);
        }

        public static AvailableBook CirculatingAvailableBookAt(BookId bookId, LibraryBranchId libraryBranchId)
        {
            return new(bookId, BookType.Circulating, Version0, libraryBranchId);
        }

        public static BookOnHold BookOnHold()
        {
            return new BookOnHold(AnyBookId, BookType.Circulating, AnyBranchId, AnyPatronId, DateTime.Now, Version0);
        }
    }
}
