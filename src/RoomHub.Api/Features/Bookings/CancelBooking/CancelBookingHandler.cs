using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public sealed class CancelBookingHandler(
    BookingRepository bookings,
    TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand, Result<Unit>>
{
    public Task<Result<Unit>> HandleAsync(
        CancelBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var booking = bookings.Get(command.Id);
        if (booking is null)
        {
            return Task.FromResult<Result<Unit>>(new Error(
                "BookingNotFound",
                "The booking does not exist.",
                ErrorType.NotFound));
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        return Task.FromResult(booking.Cancel(nowUtc));
    }
}
