using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand, Result<Unit>>
{
    public async Task<Result<Unit>> HandleAsync(
        CancelBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookings.GetByIdAsync(command.Id, cancellationToken);
        if (booking is null)
        {
            return new Error(
                "BookingNotFound",
                "The booking does not exist.",
                ErrorType.NotFound);
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var result = booking.Cancel(nowUtc);
        if (result.IsSuccess)
            await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
