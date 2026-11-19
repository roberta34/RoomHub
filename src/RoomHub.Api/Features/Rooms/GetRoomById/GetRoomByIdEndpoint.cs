using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public record Response(Guid Id, string Name, int Capacity);

    public static void MapGetRoomByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", (Guid id, RoomService service) =>
        {
            var room = service.Get(id);
            return Results.Ok(new Response(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
