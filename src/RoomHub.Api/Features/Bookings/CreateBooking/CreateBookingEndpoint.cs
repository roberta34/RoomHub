using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

    public record Response(Guid Id, Guid RoomId, string Status);

    public static void MapCreateBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", (Guid roomId, Request request, BookingService service) =>
        {
            var result = service.Create(
                roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var booking = result.Value!;
            return Results.Created(
                $"/api/bookings/{booking.Id}",
                new Response(booking.Id, booking.RoomId, booking.Status.ToString()));
        }).WithTags("Bookings");
    }
}
