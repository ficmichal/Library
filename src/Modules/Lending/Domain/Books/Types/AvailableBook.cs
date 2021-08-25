﻿using Library.BuildingBlocks.Domain;
using Library.Modules.Lending.Domain.LibraryBranch;
using System.Collections.Generic;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.Domain.Books.Types
{
    public class AvailableBook : ValueObject, IBook
    {
        public AvailableBook(BookId id, BookType type, Version version, LibraryBranchId libraryBranchId)
        {
            Id = id;
            Type = type;
            Information = new BookInformation(id, type);
            Version = version;
            LibraryBranchId = libraryBranchId;
        }

        public BookId Id { get; }

        public BookType Type { get; }

        public BookInformation Information { get; }

        public LibraryBranchId LibraryBranchId { get; }

        
        public Version Version { get; }

        public bool IsRestricted()
        {
            return Information.Type.Equals(BookType.Restricted);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Information;
        }
    }
}
