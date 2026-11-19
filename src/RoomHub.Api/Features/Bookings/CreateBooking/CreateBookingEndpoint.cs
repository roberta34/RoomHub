using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public static class CreateBookingEndpoint
{
    public record Request(string? BookedBy, DateTime StartUtc, DateTime EndUtc, int Attendees);

    public record Response(Guid Id, Guid RoomId, string Status);

    public static void MapCreateBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
            Guid roomId,
            Request request,
            ICommandHandler<CreateBookingCommand, Result<Booking>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateBookingCommand(
                    roomId,
                    request.BookedBy,
                    request.StartUtc,
                    request.EndUtc,
                    request.Attendees),
                cancellationToken);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var booking = result.Value!;
            return TypedResults.Created(
                $"/api/bookings/{booking.Id}",
                new Response(booking.Id, booking.RoomId, booking.Status.ToString()));
        }).WithTags("Bookings");
    }
}
