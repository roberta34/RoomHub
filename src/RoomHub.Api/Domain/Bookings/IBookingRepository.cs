namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetActiveByRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetActiveByParticipantAndDateAsync(
        string bookedBy,
        DateTime utcDate,
        CancellationToken cancellationToken = default);
    void Add(Booking booking);
}
