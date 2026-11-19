namespace RoomHub.Api.Domain.Common;

public sealed record DateRange
{
    public DateTime StartUtc { get; private init; }
    public DateTime EndUtc { get; private init; }

    private DateRange()
    {
    }

    private DateRange(DateTime startUtc, DateTime endUtc)
    {
        StartUtc = startUtc;
        EndUtc = endUtc;
    }

    public static Result<DateRange> Create(DateTime startUtc, DateTime endUtc)
    {
        if (endUtc <= startUtc)
        {
            return new Error(
                "InvalidRange",
                "The end must be after the start.",
                ErrorType.Validation);
        }

        return new DateRange(startUtc, endUtc);
    }

    public bool Overlaps(DateRange other) =>
        StartUtc < other.EndUtc && other.StartUtc < EndUtc;
}
