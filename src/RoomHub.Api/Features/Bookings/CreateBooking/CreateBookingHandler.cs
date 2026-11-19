using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<Booking>>
{
    public Task<Result<Booking>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = rooms.Get(command.RoomId);
        if (room is null)
        {
            return Task.FromResult<Result<Booking>>(new Error(
                "RoomNotFound",
                "The room does not exist.",
                ErrorType.NotFound));
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var result = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            bookings.ListForRoom(command.RoomId),
            bookings.ListForParticipant(command.BookedBy?.Trim() ?? string.Empty),
            nowUtc);

        if (result.IsSuccess)
            bookings.Add(result.Value!);

        return Task.FromResult(result);
    }
}
