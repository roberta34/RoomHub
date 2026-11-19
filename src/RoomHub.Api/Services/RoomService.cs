using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Services;

public class RoomService(RoomRepository rooms)
{
    public Result<Room> Create(string? name, int capacity)
    {
        var result = Room.Create(name, capacity);
        if (result.IsSuccess)
            rooms.Add(result.Value!);

        return result;
    }

    public Result<Room> Get(Guid id)
    {
        var room = rooms.Get(id);
        if (room is null)
            return new Error("RoomNotFound", "The room does not exist.", ErrorType.NotFound);

        return room;
    }
}
