using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Features.Bookings.GetBookingById;

public static class GetBookingByIdEndpoint
{
    public record Response(
        Guid Id,
        Guid RoomId,
        string BookedBy,
        DateTime StartUtc,
        DateTime EndUtc,
        int Attendees,
        string Status);

    public static void MapGetBookingByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            RoomHubDbContext db,
            CancellationToken cancellationToken) =>
        {
            var booking = await db.Bookings
                .AsNoTracking()
                .Where(value => value.Id == id)
                .Select(value => new Response(
                    value.Id,
                    value.RoomId,
                    value.BookedBy,
                    value.Range.StartUtc,
                    value.Range.EndUtc,
                    value.Attendees,
                    value.Status.ToString()))
                .FirstOrDefaultAsync(cancellationToken);

            return booking is null
                ? new Error("BookingNotFound", "The booking does not exist.", ErrorType.NotFound).ToProblem()
                : TypedResults.Ok(booking);
        }).WithTags("Bookings");
    }
}
