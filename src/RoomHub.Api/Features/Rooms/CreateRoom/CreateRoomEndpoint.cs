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
            var result = service.Create(request.Name, request.Capacity);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var room = result.Value!;
            return Results.Created(
                $"/api/rooms/{room.Id}",
                new Response(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
