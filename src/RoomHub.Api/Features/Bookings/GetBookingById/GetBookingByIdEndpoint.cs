using RoomHub.Api.Services;

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
        app.MapGet("/api/bookings/{id:guid}", (Guid id, BookingService service) =>
        {
            var result = service.Get(id);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var booking = result.Value!;
            return Results.Ok(new Response(
                booking.Id,
                booking.RoomId,
                booking.BookedBy,
                booking.StartUtc,
                booking.EndUtc,
                booking.Attendees,
                booking.Status.ToString()));
        }).WithTags("Bookings");
    }
}
