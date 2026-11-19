using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Services;

public class BookingService(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider timeProvider)
{
    public Result<Booking> Create(Guid roomId, string? bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        var room = rooms.Get(roomId);
        if (room is null)
            return new Error("RoomNotFound", "The room does not exist.", ErrorType.NotFound);

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var result = Booking.Create(
            room,
            bookedBy,
            startUtc,
            endUtc,
            attendees,
            bookings.ListForRoom(roomId),
            bookings.ListForParticipant(bookedBy?.Trim() ?? string.Empty),
            nowUtc);

        if (result.IsSuccess)
            bookings.Add(result.Value!);

        return result;
    }

    public Result<Booking> Get(Guid id)
    {
        var booking = bookings.Get(id);
        if (booking is null)
            return new Error("BookingNotFound", "The booking does not exist.", ErrorType.NotFound);

        return booking;
    }

    public Result<Unit> Cancel(Guid id)
    {
        var result = Get(id);
        if (result.IsFailure)
            return result.Error!;

        return result.Value!.Cancel();
    }
}
