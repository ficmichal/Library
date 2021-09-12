using System.Threading.Tasks;
using Library.BuildingBlocks.Domain.Commands;

namespace Library.Modules.Lending.Application.Patrons.Hold
{
    public interface ICancelingHold
    {
        Task<Result> CancelHold(CancelHoldCommand command);
    }
}
