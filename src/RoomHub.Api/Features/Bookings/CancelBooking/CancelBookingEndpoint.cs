using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", (Guid id, BookingService service) =>
        {
            var result = service.Cancel(id);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            return Results.NoContent();
        }).WithTags("Bookings");
    }
}
