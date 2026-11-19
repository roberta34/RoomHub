using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Infrastructure.Persistence;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public record Response(Guid Id, string Name, int Capacity);

    public static void MapGetRoomByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", async (
            Guid id,
            RoomHubDbContext db,
            CancellationToken cancellationToken) =>
        {
            var room = await db.Rooms
                .AsNoTracking()
                .Where(value => value.Id == id)
                .Select(value => new Response(value.Id, value.Name, value.Capacity))
                .FirstOrDefaultAsync(cancellationToken);

            return room is null
                ? new Error("RoomNotFound", "The room does not exist.", ErrorType.NotFound).ToProblem()
                : TypedResults.Ok(room);
        }).WithTags("Rooms");
    }
}
