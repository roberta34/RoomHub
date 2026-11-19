using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public record Request(string? Name, int Capacity);

    public record Response(Guid Id, string Name, int Capacity);

    public static void MapCreateRoomEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (
            Request request,
            ICommandHandler<CreateRoomCommand, Result<Room>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateRoomCommand(request.Name, request.Capacity),
                cancellationToken);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var room = result.Value!;
            return TypedResults.Created(
                $"/api/rooms/{room.Id}",
                new Response(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
