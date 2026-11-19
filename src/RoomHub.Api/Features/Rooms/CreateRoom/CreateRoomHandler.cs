using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(RoomRepository rooms)
    : ICommandHandler<CreateRoomCommand, Result<Room>>
{
    public Task<Result<Room>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsSuccess)
            rooms.Add(result.Value!);

        return Task.FromResult(result);
    }
}
