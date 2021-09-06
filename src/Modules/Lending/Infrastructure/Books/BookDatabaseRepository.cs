using Dapper;
using Library.BuildingBlocks.Domain.Exceptions;
using Library.BuildingBlocks.Infrastructure.Data;
using Library.Modules.Lending.Domain.Books;
using Library.Modules.Lending.Domain.Books.Types;
using System;
using System.Threading.Tasks;
using Version = Library.BuildingBlocks.Domain.Version;

namespace Library.Modules.Lending.Infrastructure.Books
{
    public class BookDatabaseRepository : IBookRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public BookDatabaseRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IBook> FindBy(BookId id)
        {
            return (await FindBookById(id))?.ToDomainModel();
        }

        private async Task<BookDatabaseEntity> FindBookById(BookId bookId)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            return await connection.QuerySingleOrDefaultAsync<BookDatabaseEntity>(
                "SELECT " +
                $"[Book].[Id] AS [{nameof(BookDatabaseEntity.Id)}], " +
                $"[Book].[BookId] AS [{nameof(BookDatabaseEntity.BookId)}], " +
                $"[Book].[BookType] AS [{nameof(BookDatabaseEntity.BookType)}], " +
                $"[Book].[BookState] AS [{nameof(BookDatabaseEntity.BookState)}], " +
                $"[Book].[AvailableAtBranch] AS [{nameof(BookDatabaseEntity.AvailableAtBranch)}], " +
                $"[Book].[OnHoldAtBranch] AS [{nameof(BookDatabaseEntity.OnHoldAtBranch)}], " +
                $"[Book].[OnHoldByPatron] AS [{nameof(BookDatabaseEntity.OnHoldByPatron)}], " +
                $"[Book].[OnHoldTill] AS [{nameof(BookDatabaseEntity.OnHoldTill)}], " +
                $"[Book].[Version] AS [{nameof(BookDatabaseEntity.Version)}] " +
                $"FROM {nameof(BookDatabaseEntity)} AS [Book] " +
                "WHERE [Book].[BookId] = @BookId",
                new
                {
                    BookId = bookId.Id
                });
        }

        public async Task Save(IBook book)
        {
            var dbBook = await FindBy(book.Id);

            if (dbBook is null)
            {
                await InsertNew(book);
                return;
            }

            await UpdateOptimistically(book);
        }

        private async Task UpdateOptimistically(IBook book)
        {
            var result = book switch
            {
                AvailableBook availableBook => await Update(availableBook),
                BookOnHold bookOnHold => await Update(bookOnHold),
                _ => 0
            };

            if (result == 0)
            {
                throw new AggregateRootIsStale($"Someone has updated the book in the meantime, bookId: {book.Id}");
            }
        }

        private async Task<int> Update(AvailableBook book)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var sql = $"UPDATE {nameof(BookDatabaseEntity)} SET " +
                      $"[{nameof(BookDatabaseEntity.BookState)}] = @BookState, " +
                      $"[{nameof(BookDatabaseEntity.AvailableAtBranch)}] = @AvailableAtBranch, " +
                      $"[{nameof(BookDatabaseEntity.Version)}] = @Version " +
                      $"WHERE [{nameof(BookDatabaseEntity.BookId)}] = @BookId AND [{nameof(BookDatabaseEntity.Version)}] = @OldVersion";

            return await connection.ExecuteAsync(sql,
                new
                {
                    BookState = BookState.Available,
                    AvailableAtBranch = book.LibraryBranchId.Id,
                    OldVersion = book.Version.Value,
                    Version = book.Version.Value + 1,
                    BookId = book.Id.Id
                });
        }

        private async Task<int> Update(BookOnHold book)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var sql = $"UPDATE {nameof(BookDatabaseEntity)} SET " +
                      $"[{nameof(BookDatabaseEntity.BookState)}] = @BookState, " +
                      $"[{nameof(BookDatabaseEntity.OnHoldAtBranch)}] = @OnHoldAtBranch, " +
                      $"[{nameof(BookDatabaseEntity.OnHoldByPatron)}] = @OnHoldByPatron, " +
                      $"[{nameof(BookDatabaseEntity.OnHoldTill)}] = @OnHoldTill, " +
                      $"[{nameof(BookDatabaseEntity.Version)}] = @Version " +
                      $"WHERE [{nameof(BookDatabaseEntity.BookId)}] = @BookId AND [{nameof(BookDatabaseEntity.Version)}] = @OldVersion";

            return await connection.ExecuteAsync(sql,
                new
                {
                    BookState = BookState.OnHold,
                    OnHoldAtBranch = book.HoldPlacedAt.Id,
                    OnHoldByPatron = book.ByPatron.Id,
                    OnHoldTill = book.HoldTill,
                    OldVersion = book.Version.Value,
                    Version = book.Version.Value + 1,
                    BookId = book.Id.Id
                });
        }

        private async Task InsertNew(IBook book)
        {
            _ = book switch
            {
                AvailableBook availableBook => await Insert(availableBook),
                BookOnHold bookOnHold => await Insert(bookOnHold),
                _ => 0
            };
        }

        private async Task<int> Insert(AvailableBook book)
        {
            return await Insert(book.Id, book.Type, BookState.Available, book.LibraryBranchId.Id, null, null, null);
        }

        private async Task<int> Insert(BookOnHold book)
        {
            return await Insert(book.Id, book.Type, BookState.OnHold, null, book.HoldPlacedAt.Id, book.ByPatron.Id,
                book.HoldTill);
        }

        private async Task<int> Insert(BookId bookId, BookType bookType, BookState bookState,
            Guid? availableAt, Guid? onHoldAt, Guid? onHoldBy, DateTime? onHoldTill)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            var sql = $"SET IDENTITY_INSERT {nameof(BookDatabaseEntity)} ON; " +
                      $"INSERT INTO {nameof(BookDatabaseEntity)}" +
                               $"([{nameof(BookDatabaseEntity.Id)}]," +
                               $" [{nameof(BookDatabaseEntity.BookId)}]," +
                               $" [{nameof(BookDatabaseEntity.BookType)}]," +
                               $" [{nameof(BookDatabaseEntity.BookState)}]," +
                               $" [{nameof(BookDatabaseEntity.AvailableAtBranch)}]," +
                               $" [{nameof(BookDatabaseEntity.OnHoldAtBranch)}]," +
                               $" [{nameof(BookDatabaseEntity.OnHoldByPatron)}]," +
                               $" [{nameof(BookDatabaseEntity.OnHoldTill)}]," +
                               $" [{nameof(BookDatabaseEntity.Version)}]) VALUES " +
                               "(NEXT VALUE FOR BookDatabaseEntitySeq," +
                               " @BookId," +
                               " @BookType," +
                               " @BookState," +
                               " @AvailableAtBranch," +
                               " @OnHoldAtBranch," +
                               " @OnHoldByPatron," +
                               " @OnHoldTill," +
                               " @Version); " +
                      $"SET IDENTITY_INSERT {nameof(BookDatabaseEntity)} OFF;";

            return await connection.ExecuteAsync(sql,
                new
                {
                    BookId = bookId.Id,
                    BookType = bookType,
                    BookState = bookState,
                    AvailableAtBranch = availableAt,
                    OnHoldAtBranch = onHoldAt,
                    OnHoldByPatron = onHoldBy,
                    OnHoldTill = onHoldTill,
                    Version = Version.Zero().Value
                });
        }
    }
}
