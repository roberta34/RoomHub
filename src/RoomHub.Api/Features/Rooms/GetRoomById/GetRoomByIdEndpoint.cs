using RoomHub.Api.Services;

namespace RoomHub.Api.Features.Rooms.GetRoomById;

public static class GetRoomByIdEndpoint
{
    public record Response(Guid Id, string Name, int Capacity);

    public static void MapGetRoomByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", (Guid id, RoomService service) =>
        {
            var result = service.Get(id);
            if (result.IsFailure)
                return result.Error!.ToProblem();

            var room = result.Value!;
            return Results.Ok(new Response(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
