using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public static class CreateRoomEndpoint
{
    public record Request(string? Name, int Capacity);

    public record Response(Guid Id, string Name, int Capacity);

    public static void MapCreateRoomEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", (Request request, RoomService service) =>
        {
            var room = service.Create(request.Name, request.Capacity);
            return Results.Created(
                $"/api/rooms/{room.Id}",
                new Response(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
