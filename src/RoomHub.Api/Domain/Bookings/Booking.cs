using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public enum BookingStatus
{
    Active,
    Cancelled
}

public sealed class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public DateRange Range { get; private set; } = null!;
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking()
    {
    }

    private Booking(
        Guid id,
        Guid roomId,
        string bookedBy,
        DateRange range,
        int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        Range = range;
        StartUtc = range.StartUtc;
        EndUtc = range.EndUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        IReadOnlyCollection<Booking> roomBookings,
        IReadOnlyCollection<Booking> participantBookings,
        DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return new Error("InvalidBookedBy", "BookedBy is required.", ErrorType.Validation);

        var rangeResult = DateRange.Create(startUtc, endUtc);
        if (rangeResult.IsFailure)
            return rangeResult.Error!;

        if (attendees <= 0)
            return new Error("InvalidAttendees", "Attendees must be positive.", ErrorType.Validation);

        if (attendees > room.Capacity)
            return new Error("AttendeesExceedCapacity", "The room does not have enough seats.", ErrorType.Validation);

        if (startUtc < nowUtc)
            return new Error("BookingInPast", "The booking cannot start in the past.", ErrorType.Validation);

        var maximumDuration = room.Capacity > 10
            ? TimeSpan.FromHours(2)
            : TimeSpan.FromHours(4);

        if (endUtc - startUtc > maximumDuration)
            return new Error("InvalidDuration", "The booking duration exceeds the room limit.", ErrorType.Validation);

        var range = rangeResult.Value!;
        var participantName = bookedBy.Trim();
        var activeBookingsForParticipantOnDay = participantBookings.Count(existing =>
            existing.Status == BookingStatus.Active &&
            string.Equals(existing.BookedBy, participantName, StringComparison.Ordinal) &&
            existing.StartUtc.Date == startUtc.Date);

        if (activeBookingsForParticipantOnDay >= 2)
        {
            return new Error(
                "ParticipantDailyBookingLimitExceeded",
                "The participant already has two active bookings on this UTC day.",
                ErrorType.Conflict);
        }

        foreach (var existing in roomBookings)
        {
            if (existing.Status == BookingStatus.Active &&
                existing.RoomId == room.Id &&
                range.Overlaps(existing.Range))
            {
                return new Error("RoomOccupied", "The room is occupied in this interval.", ErrorType.Conflict);
            }
        }

        return new Booking(Guid.NewGuid(), room.Id, bookedBy.Trim(), range, attendees);
    }

    public Result<Unit> Cancel(DateTime nowUtc)
    {
        if (Status == BookingStatus.Cancelled)
        {
            return new Error("BookingAlreadyCancelled", "The booking is already cancelled.", ErrorType.Conflict);
        }

        if (StartUtc < nowUtc.AddMinutes(60))
        {
            return new Error(
                "CancellationTooLate",
                "The booking can only be cancelled at least 60 minutes before it starts.",
                ErrorType.Conflict);
        }

        Status = BookingStatus.Cancelled;
        return Unit.Value;
    }
}
