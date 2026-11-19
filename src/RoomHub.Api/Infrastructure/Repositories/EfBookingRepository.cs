using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class EfBookingRepository(RoomHubDbContext db) : IBookingRepository
{
    public void Add(Booking booking) => db.Bookings.Add(booking);

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        db.Bookings.FirstOrDefaultAsync(booking => booking.Id == id, cancellationToken);

    public Task<List<Booking>> GetActiveByRoomAsync(
        Guid roomId,
        CancellationToken cancellationToken = default) =>
        db.Bookings
            .Where(booking => booking.RoomId == roomId &&
                              booking.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);

    public Task<List<Booking>> GetActiveByParticipantAndDateAsync(
        string bookedBy,
        DateTime utcDate,
        CancellationToken cancellationToken = default)
    {
        var start = utcDate.Date;
        var end = start.AddDays(1);

        return db.Bookings
            .Where(booking =>
                booking.Status == BookingStatus.Active &&
                booking.BookedBy == bookedBy &&
                booking.Range.StartUtc >= start &&
                booking.Range.StartUtc < end)
            .ToListAsync(cancellationToken);
    }
}
