using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<Booking>>
{
    public async Task<Result<Booking>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
        {
            return new Error(
                "RoomNotFound",
                "The room does not exist.",
                ErrorType.NotFound);
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        var result = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            await bookings.GetActiveByRoomAsync(command.RoomId, cancellationToken),
            await bookings.GetActiveByParticipantAndDateAsync(
                command.BookedBy?.Trim() ?? string.Empty,
                command.StartUtc,
                cancellationToken),
            nowUtc);

        if (result.IsSuccess)
        {
            bookings.Add(result.Value!);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return result;
    }
}
