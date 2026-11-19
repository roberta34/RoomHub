using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand, Result<Unit>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CancelBookingCommand(id),
                cancellationToken);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            return TypedResults.NoContent();
        }).WithTags("Bookings");
    }
}
